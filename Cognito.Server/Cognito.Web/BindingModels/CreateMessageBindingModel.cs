using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class CreateMessageBindingModel
    {
        [Required]
        public int? SenderId { get; set; }
        [Required]
        public int? RecipientId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}