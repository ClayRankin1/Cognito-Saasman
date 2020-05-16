using Cognito.Web.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Document
{
    public class LinkDocumentBindingModel
    {
        [Required]
        [TaskAccessPermission]
        public int? TaskId { get; set; }
    }
}
