using Cognito.DataAccess.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Task
{
    public class TaskBindingModel
    {
        [Required]
        public int? ProjectId { get; set; }

        [Required]
        public int? OwnerId { get; set; }

        [Required]
        public int? TaskTypeId { get; set; }

        [Required]
        public DateTime? NextDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public int? TimeId { get; set; }

        [Required]
        public TaskStatusId? Status { get; set; }

        public string Description { get; set; }

        [Required]
        public bool? IsEvent { get; set; }

        public string Accrued { get; set; }

        public int[] Subtasks { get; set; }
    }
}
