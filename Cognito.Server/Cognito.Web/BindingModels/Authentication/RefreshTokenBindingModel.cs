using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Authentication
{
    public class RefreshTokenBindingModel
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
