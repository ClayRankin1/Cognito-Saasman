using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Task
{
    public class TaskIdBindingModel
    {
        [Required]
        public int? TaskId { get; set; }
    }
}
