using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Authentication
{
    public class LogoutBindingModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
