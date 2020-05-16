using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class Domain : AuditableEntityBase
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public int? AdminUserId { get; set; }

        public int? TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<UserDomain> UserDomains { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public virtual ICollection<DomainLicense> DomainLicenses { get; set; }
    }
}
