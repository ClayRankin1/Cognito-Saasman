using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Task
{
    public class MoveTaskBindingModel : TaskIdBindingModel
    {
        [Required]
        public DateTime? NextDate { get; set; }
    }
}
