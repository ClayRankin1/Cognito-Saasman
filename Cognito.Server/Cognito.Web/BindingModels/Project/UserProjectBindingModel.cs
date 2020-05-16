using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Project
{
    public class UserProjectBindingModel
    {
        [Required]
        public int? UserId { get; set; }

        public int? ProjectId { get; set; }
    }
}
