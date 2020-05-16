using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Point
{
    public class UpdatePointBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}