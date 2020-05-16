using Cognito.DataAccess.Entities;
using Cognito.Web.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Document
{
    public class DocumentBindingModel
    {
        [Required]
        [TaskAccessPermission]
        public int? TaskId { get; set; }

        [Required]
        public DocumentStatusId? Status { get; set; }

        public string Description { get; set; }
    }
}