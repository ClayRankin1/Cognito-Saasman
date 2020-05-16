using System;

namespace Cognito.Business.ViewModels
{
    public class TaskViewModel : AuditableViewModel
    {
        public int ProjectId { get; set; }

        public int OwnerId { get; set; }

        public string Description { get; set; }

        public string GroupDate { get; set; }

        public DateTime NextDate { get; set; }

        public string NextTime { get; set; }

        public DateTime? EndDate { get; set; }

        public int[] Subtasks { get; set; }

        public int TaskTypeId { get; set; }

        public int Status { get; set; }

        public string Accrued { get; set; }

        public decimal AccruedTotal { get; set; }

        public string ProjectName { get; set; }

        public int? ProjectTypeId { get; set; }

        public int DetailsCount { get; set; }

        public int? TimeId { get; set; }

        public bool IsEvent { get; set; }
    }
}
