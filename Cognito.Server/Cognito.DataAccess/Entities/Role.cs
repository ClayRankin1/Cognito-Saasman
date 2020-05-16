using Cognito.DataAccess.Attributes;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(Role), Schema = DbSchemas.User)]
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
