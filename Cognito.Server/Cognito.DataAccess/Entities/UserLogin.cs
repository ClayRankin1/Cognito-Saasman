using Cognito.DataAccess.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(UserLogin), Schema = DbSchemas.User)]
    public class UserLogin : IdentityUserLogin<int>
    {

    }
}
