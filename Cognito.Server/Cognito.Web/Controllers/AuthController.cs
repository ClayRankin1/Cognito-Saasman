using AutoMapper;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels;
using Cognito.Web.BindingModels.Authentication;
using Cognito.Web.Infrastructure.Filters;
using Cognito.Web.Services.Abstract;
using Cognito.Web.Services.Security.Abstract;
using Cognito.Web.ViewModels.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AuthController(
            UserManager<User> userManager,
            IMapper mapper,
            IEmailSender emailSender,
            IUserService userService,
            IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpPost("password-reset-email")]
        public async Task<IActionResult> SendPasswordResetEmail(PasswordResetBindingModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var response = await _emailSender.SendResetPasswordEmailAsync(model.Email, token);
            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return Ok();
            }

            return BadRequest(new { message = "Unable to send email message" });
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordBindingModel model)
        {
            var result = await _userService.ResetPasswordAsync(model.Email, model.Password, model.Token, true);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationBindingModel model)
        {
            var userToCreate = new User
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var domain = new Domain
            {
                Name = model.DomainName
            };

            var domainUser = new UserDomain()
            {
                Domain = domain,
                User = userToCreate
            };
            userToCreate.UserDomains.Add(domainUser);

            var result = await _userManager.CreateAsync(userToCreate, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var userToReturn = _mapper.Map<UserViewModel>(userToCreate);

            return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
        }

        /// <summary>
        /// Gets user JWT access token and refresh token based on user name and password.
        /// </summary>
        [HttpPost(nameof(Login))]
        [ProducesResponseType(typeof(TokensViewModels), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginBindingModel model)
        {
            var tokens = await _authenticationService.GetTokensAsync(model.Email, model.Password);
            return Ok(_mapper.Map<TokensViewModels>(tokens));
        }

        /// <summary>
        /// Refreshes user's JWT access token and refresh token.
        /// </summary>
        [TransactionScope]
        [HttpPost(nameof(Refresh))]
        [ProducesResponseType(typeof(TokensViewModels), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh(RefreshTokenBindingModel model)
        {
            var tokens = await _authenticationService.GetRefreshTokenAsync(model.AccessToken, model.RefreshToken);
            return Ok(_mapper.Map<TokensViewModels>(tokens));
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        [Authorize]
        [HttpPost(nameof(Logout))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout(LogoutBindingModel model)
        {
            await _authenticationService.LogoutUserAsync(model.RefreshToken);
            return Ok();
        }
    }
}