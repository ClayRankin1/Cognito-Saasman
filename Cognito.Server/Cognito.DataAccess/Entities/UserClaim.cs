using Cognito.DataAccess.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(UserClaim), Schema = DbSchemas.User)]
    public class UserClaim : IdentityUserClaim<int>
    {
    }
}
