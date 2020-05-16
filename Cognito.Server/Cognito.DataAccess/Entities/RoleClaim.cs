using Cognito.DataAccess.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(RoleClaim), Schema = DbSchemas.User)]
    public class RoleClaim : IdentityRoleClaim<int>
    {
    }
}
