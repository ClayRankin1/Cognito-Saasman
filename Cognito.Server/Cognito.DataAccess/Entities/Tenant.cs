using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class Tenant : AuditableEntityBase
    {
        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        [Required]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContactName { get; set; }

        [Required]
        [MaxLength(128)]
        public string Email { get; set; }

        [Required]
        [MaxLength(25)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string TenantName { get; set; }

        public virtual ICollection<Domain> Domains { get; set; }
    }
}
