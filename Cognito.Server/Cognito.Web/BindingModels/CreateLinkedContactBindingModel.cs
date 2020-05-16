using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class CreateLinkedContactBindingModel
    {
        [Required]
        public int? ContactId { get; set; }
        [Required]
        public int? ActId { get; set; }
        [Required]
        public int? MatterId { get; set; }
        [Required]
        public int? DomainId { get; set; }
    }
}