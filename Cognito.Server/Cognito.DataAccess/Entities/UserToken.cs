using Cognito.DataAccess.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(UserToken), Schema = DbSchemas.User)]
    public class UserToken : IdentityUserToken<int>
    {
    }
}
