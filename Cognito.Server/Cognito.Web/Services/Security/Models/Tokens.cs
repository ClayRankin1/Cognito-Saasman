namespace Cognito.Web.Services.Security.Models
{
    public class Tokens
    {
        public string AccessToken { get; set; }

        public GeneratedRefreshToken RefreshToken { get; set; }
    }
}
