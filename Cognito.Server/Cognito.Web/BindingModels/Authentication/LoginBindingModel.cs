using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Authentication
{
    public class LoginBindingModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
