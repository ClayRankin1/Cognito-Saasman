using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.Web.ResponseModels;
using Cognito.Web.Services.Abstract;
using Cognito.Web.Services.Security.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Cognito.Web.Services
{
    public class UserService : IUserService
    {
        private readonly ICognitoDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordService _passwordService;
        private readonly UserManager<User> _userManager;

        public UserService(
            ICognitoDbContext context,
            IEmailSender emailSender,
            IPasswordService passwordService,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _passwordService = passwordService;
        }

        public async Task<CreateUserIdentityResult> CreateUserAsync(string email, int domainId)
        {
            var password = _passwordService.GetRandomPassword();
            var userResult = await _userManager.CreateAsync(new User
            {
                UserName = email,
                Email = email
            }, password);

            if (!userResult.Succeeded)
            {
                return new CreateUserIdentityResult(userResult, null);
            }

            var user = await _userManager.FindByEmailAsync(email);

            _context.UserDomains.Add(new UserDomain
            {
                UserId = user.Id,
                DomainId = domainId
            });
            await _context.SaveChangesAsync();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            await _emailSender.SendInvitationEmailAsync(user.Email, token);

            return new CreateUserIdentityResult(userResult, user);
        }

        public async Task<UserIdentityResult> ResetPasswordAsync(string email, string password, string token, bool isEmailConfirmed = false)
        {
            var user = await _userManager.FindByEmailAsync(email);
            // no errors => we dont want to let the user know that email address does not exists
            // security reasons
            if (user == null)
            {
                return new UserIdentityResult();
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded && isEmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _context.SaveChangesAsync();

                return new UserIdentityResult(result);
            }

            return new UserIdentityResult();
        }

        public async Task<UserIdentityResult> DeleteUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.DeleteAsync(user);
            return new UserIdentityResult(result);
        }
    }
}
