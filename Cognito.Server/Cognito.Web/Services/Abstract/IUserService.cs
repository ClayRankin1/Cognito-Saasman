using Cognito.Web.ResponseModels;
using System.Threading.Tasks;

namespace Cognito.Web.Services.Abstract
{
    public interface IUserService
    {
        Task<CreateUserIdentityResult> CreateUserAsync(string email, int domainId);

        Task<UserIdentityResult> ResetPasswordAsync(string email, string password, string token, bool isEmailConfirmed = false);

        Task<UserIdentityResult> DeleteUserAsync(string email);
    }
}
