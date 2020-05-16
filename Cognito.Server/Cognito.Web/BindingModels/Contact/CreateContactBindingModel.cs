using Cognito.Web.BindingModels.Contact;
using Cognito.Web.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class CreateContactBindingModel : UpdateContactBindingModel
    {
        [Required]
        [TaskAccessPermission]
        public int? TaskId { get; set; }
    }
}
