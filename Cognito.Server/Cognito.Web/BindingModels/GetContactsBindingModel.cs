using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class GetContactsBindingModel
    {
        [Required]
        public int? Key { get; set; }
        [Required]
        public int? ActId { get; set; }
    }
}