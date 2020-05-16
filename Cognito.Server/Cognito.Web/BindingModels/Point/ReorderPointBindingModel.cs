using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Point
{
    public class ReorderPointBindingModel
    {
        [Required]
        public int? Count { get; set; }

        public int? ParentId { get; set; } = null;
    }
}