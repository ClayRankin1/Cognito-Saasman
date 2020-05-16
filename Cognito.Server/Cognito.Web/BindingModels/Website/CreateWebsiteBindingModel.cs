using Cognito.Web.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Website
{
    public class CreateWebsiteBindingModel : UpdateWebsiteBindingModel
    {
        [Required]
        [TaskAccessPermission]
        public int? TaskId { get; set; }
    }
}
