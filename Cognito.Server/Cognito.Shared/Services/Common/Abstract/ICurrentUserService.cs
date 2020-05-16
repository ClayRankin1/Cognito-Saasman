using Cognito.Shared.Security;
using System.Security.Claims;

namespace Cognito.Shared.Services.Common.Abstract
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the user's roles.
        /// </summary>
        string[] Roles { get; }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        ClaimsPrincipal User { get; }

        /// <summary>
        /// Gets the user's ID.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Gets the user's Domain unique indetifier.
        /// </summary>
        int DomainId { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Retrieves the value of a given claim.
        /// </summary>
        /// <param name="claim">Claim name.</param>
        /// <returns>Claim.</returns>
        string GetClaimValue(string claim);

        /// <summary>
        /// Checks if user is in one of given roles.
        /// </summary>
        /// <param name="roles">User roles.</param>
        /// <returns>A value indicating whether the user is in one of the roles.</returns>
        bool IsInOneOfRoles(params UserRoles[] roles);

        /// <summary>
        /// Checks if the user is in the given role.
        /// </summary>
        /// <param name="role">User role.</param>
        /// <returns>A value indicating whether the user has the given role.</returns>
        bool IsInRole(UserRoles role);
    }
}
