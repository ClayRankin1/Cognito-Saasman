using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class CreateItemDocumentBindingModel
    {
        [Required]
        public int? DocId { get; set; }
        [Required]
        public int? ActId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public int? MatterId { get; set; }
        [Required]
        public int? DomainId { get; set; }
        [Required]
        public string Key { get; set; }
    }
}