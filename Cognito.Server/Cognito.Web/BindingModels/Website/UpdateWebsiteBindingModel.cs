using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Website
{
    public class UpdateWebsiteBindingModel
    {
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Url { get; set; }
    }
}
