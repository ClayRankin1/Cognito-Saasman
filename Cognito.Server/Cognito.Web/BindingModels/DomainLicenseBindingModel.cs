using Cognito.DataAccess.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class DomainLicenseBindingModel
    {
        public int? DomainId { get; set; }

        [Required]
        public int? LicenseId { get; set; }

        [Required]
        [Range(0, 100)]
        public int? Discount { get; set; }

        [Required]
        [DecimalPrecision(18, 2)]
        public decimal? Price { get; set; }

        [Required]
        public int? Licenses { get; set; }
    }
}
