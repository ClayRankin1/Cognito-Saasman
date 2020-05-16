using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(TaskContact), Schema = DbSchemas.Task)]
    public class TaskContact : ITaskable
    {
        [Required]
        public int TaskId { get; set; }

        public virtual ProjectTask Task { get; set; }

        [Required]
        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
