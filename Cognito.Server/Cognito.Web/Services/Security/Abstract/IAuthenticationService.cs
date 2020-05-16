using Cognito.Web.Services.Security.Models;
using System.Threading.Tasks;

namespace Cognito.Web.Services.Security.Abstract
{
    public interface IAuthenticationService
    {
        Task<Tokens> GetTokensAsync(string email, string password);

        Task<Tokens> GetRefreshTokenAsync(string accessToken, string refreshToken);

        Task LogoutUserAsync(string refreshToken);
    }
}
