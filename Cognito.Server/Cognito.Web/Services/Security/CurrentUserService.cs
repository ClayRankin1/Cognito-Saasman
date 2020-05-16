using Cognito.Shared.Extensions;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Cognito.Web.Services.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc/>
        public int UserId => GetClaimValue(ClaimTypes.NameIdentifier)?.ParseIntSafe() ?? 0;

        /// <inheritdoc/>
        public int DomainId => GetClaimValue(CognitoClaimTypes.DomainId)?.ParseIntSafe() ?? 0;

        /// <inheritdoc/>
        public string UserName => GetClaimValue(CognitoClaimTypes.UserName);

        /// <inheritdoc/>
        public ClaimsPrincipal User => _httpContextAccessor?.HttpContext?.User;

        /// <inheritdoc/>
        public bool IsInRole(UserRoles role) => User?.IsInRole(role.ToString()) ?? false;

        /// <inheritdoc/>
        public bool IsInOneOfRoles(params UserRoles[] roles) => roles.Any(role => User?.IsInRole(role.ToString()) ?? false);

        /// <inheritdoc/>
        public string[] Roles => User?.FindAll(ClaimTypes.Role)?.Select(c => c.Value)?.ToArray();

        /// <inheritdoc/>
        public string GetClaimValue(string claim) => User?.FindFirst(claim)?.Value;
    }
}
