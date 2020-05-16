using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class CreateUserBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int? DomainId { get; set; }
    }
}
