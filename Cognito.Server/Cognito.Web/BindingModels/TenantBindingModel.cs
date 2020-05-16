using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class TenantBindingModel
    {
        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; }

        public AddressBindingModel Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContactName { get; set; }

        [Required]
        [MaxLength(128)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(25)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string TenantName { get; set; }
    }
}
