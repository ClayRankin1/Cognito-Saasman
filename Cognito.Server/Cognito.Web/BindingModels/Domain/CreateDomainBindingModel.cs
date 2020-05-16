using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Domain
{
    public class CreateDomainBindingModel
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public virtual string AdminFirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public virtual string AdminLastName { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual string AdminEmail { get; set; }

        [Required]
        public int? TenantId { get; set; }

        [Required]
        public List<DomainLicenseBindingModel> DomainLicenses { get; set; }
    }
}
