using Cognito.DataAccess.Entities;
using System.Collections.Generic;

namespace Cognito.Web.Services.Security.Models
{
    public class UserDetails
    {
        public User User { get; }

        public IEnumerable<Role> Roles { get; }

        public IEnumerable<DomainRole> DomainRoles { get; }

        public IEnumerable<RefreshToken> RefreshTokens { get; }

        public UserDetails(
            User user,
            IEnumerable<Role> roles,
            IEnumerable<DomainRole> userDomains,
            IEnumerable<RefreshToken> refreshTokens)
        {
            User = user;
            Roles = roles;
            DomainRoles = userDomains;
            RefreshTokens = refreshTokens;
        }
    }
}
