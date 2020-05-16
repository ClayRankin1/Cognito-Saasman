using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class License : AuditableEntityBase
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [DecimalPrecision(18, 2)]
        public decimal Price { get; set; }

        public LicenseTypeId LicenseTypeId { get; set; }

        public virtual LicenseType LicenseType { get; set; }

        public virtual ICollection<DomainLicense> DomainLicenses { get; set; }
    }
}
