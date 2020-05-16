using Microsoft.AspNetCore.Identity;

namespace Cognito.Shared.Options
{
    public class SecurityOptions
    {
        public string SecurityKey { get; set; }

        public bool RequireUniqueEmail { get; set; }

        public bool RequireConfirmedEmail { get; set; }

        public int AccessTokenExpirationInMinutes { get; set; }

        public int RefreshTokenExpirationInMinutes { get; set; }

        public PasswordOptions PasswordOptions { get; set; }
    }
}
