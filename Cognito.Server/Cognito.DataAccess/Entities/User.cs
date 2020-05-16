using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(User), Schema = DbSchemas.User)]
    public class User : IdentityUser<int>, IEntity
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

        public virtual ICollection<UserDomain> UserDomains { get; set; } = new HashSet<UserDomain>();

        public virtual ICollection<Document> Documents { get; set; } = new HashSet<Document>();

        public virtual ICollection<Document> UpdatedDocuments { get; set; } = new HashSet<Document>();

        public virtual ICollection<ProjectUser> UserProjects { get; set; } = new HashSet<ProjectUser>();

        public virtual ICollection<ProjectTask> UserTasks { get; set; } = new HashSet<ProjectTask>();

        public virtual ICollection<AccruedTime> AccruedTimes { get; set; } = new HashSet<AccruedTime>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
