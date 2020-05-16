using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(TaskDocument), Schema = DbSchemas.Task)]
    public class TaskDocument : ITaskable
    {
        [Required]
        public int TaskId { get; set; }

        public virtual ProjectTask Task { get; set; }

        [Required]
        public int DocumentId { get; set; }

        public virtual Document Document { get; set; }
    }
}
