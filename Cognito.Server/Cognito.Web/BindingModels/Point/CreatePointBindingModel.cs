using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Point
{
    public class CreatePointBindingModel
    {
        [Required]
        public int? ProjectId { get; set; }
        
        [Required]
        public string Text { get; set; }

        public int? ParentId { get; set; } = null;
    }
}