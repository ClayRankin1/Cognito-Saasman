using Cognito.DataAccess.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(UserRole), Schema = DbSchemas.User)]
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
