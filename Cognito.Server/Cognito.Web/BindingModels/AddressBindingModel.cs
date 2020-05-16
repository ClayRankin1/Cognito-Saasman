using Cognito.DataAccess.Entities;
using Cognito.Web.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class AddressBindingModel
    {
        [Required]
        [MaxLength(250)]
        public string Street { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [ZipCode]
        public string Zip { get; set; }

        [Required]
        [EnumDataType(typeof(StateId))]
        public StateId? StateId { get; set; }
    }
}
