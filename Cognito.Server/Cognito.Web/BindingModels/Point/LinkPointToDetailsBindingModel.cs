using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Point
{
    public class LinkPointToDetailsBindingModel
    {
        [Required]
        public int[] DetailIds { get; set; }
    }
}