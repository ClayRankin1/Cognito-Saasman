using Cognito.Web.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Common
{
    public class IdsBindingModel
    {
        [Required]
        [MustHaveOneElement(ErrorMessage = "At least one Id is required")]
        public int[] Ids { get; set; }
    }
}
