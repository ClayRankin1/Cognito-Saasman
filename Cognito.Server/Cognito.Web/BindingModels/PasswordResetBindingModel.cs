using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class PasswordResetBindingModel
    {
        [Required]
        public string Email { get; set; }
    }
}
