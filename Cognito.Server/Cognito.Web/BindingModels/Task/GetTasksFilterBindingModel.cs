using Cognito.DataAccess.Entities;
using Cognito.Web.Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Task
{
    public class GetTasksFilterBindingModel
    {
        [Required]
        public TaskStatusId? Status { get; set; }

        [Required]
        [MustHaveOneElement]
        public int[] ProjectIds { get; set; }
    }
}
