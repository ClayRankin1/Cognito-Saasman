using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Contact
{
    public class LinkContactBindingModel
    {
        [Required]
        public int? TaskId { get; set; }

        [Required]
        public int? ContactId { get; set; }
    }
}
