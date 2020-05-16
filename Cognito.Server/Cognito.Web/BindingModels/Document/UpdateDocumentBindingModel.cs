using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Document
{
    public class UpdateDocumentBindingModel
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
