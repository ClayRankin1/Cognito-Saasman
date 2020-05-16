using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Domain
{
    public class UpdateDomainBindingModel
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        public List<DomainLicenseBindingModel> DomainLicenses { get; set; }
    }
}
