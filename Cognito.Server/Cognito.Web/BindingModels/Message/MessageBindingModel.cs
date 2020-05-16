using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Message
{
    public class MessageBindingModel
    {
        [Required]
        public int? SenderId { get; set; }

        [Required]
        public int? RecipientId { get; set; }

        public string Content { get; set; }
    }
}
