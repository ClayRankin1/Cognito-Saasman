using Cognito.Shared.Security;
using Microsoft.AspNetCore.Authorization;

namespace Cognito.Web.Infrastructure.Attributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRoles[] roles)
        {
            Roles = string.Join(',', roles);
        }
    }
}
