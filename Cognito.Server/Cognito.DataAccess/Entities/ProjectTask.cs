using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(System.Threading.Tasks.Task), Schema = DbSchemas.Task)]
    public class ProjectTask : AuditableEntityBase
    {
        [Required]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        [Required]
        public DateTime NextDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        [DecimalPrecision(4, 2)]
        public decimal? Accrued { get; set; }

        public TaskStatusId TaskStatusId { get; set; }

        public virtual TaskStatus TaskStatus { get; set; }

        public bool IsEvent { get; set; }

        public int DisplayOrder { get; set; }

        [Required]
        public int TimeId { get; set; }

        public virtual Time Time { get; set; }

        [Required]
        public int TaskTypeId { get; set; }

        public virtual TaskType TaskType { get; set; }

        public virtual ICollection<Detail> Details { get; set; } = new HashSet<Detail>();

        public virtual ICollection<AccruedTime> AccruedTimes { get; set; } = new HashSet<AccruedTime>();

        public virtual ICollection<Subtask> Subtasks { get; set; } = new HashSet<Subtask>();

        public virtual ICollection<TaskWebsite> TaskWebsites { get; set; } = new HashSet<TaskWebsite>();

        public virtual ICollection<TaskContact> TaskContacts { get; set; } = new HashSet<TaskContact>();

        public virtual ICollection<TaskDocument> TaskDocuments { get; set; } = new HashSet<TaskDocument>();
    }
}
