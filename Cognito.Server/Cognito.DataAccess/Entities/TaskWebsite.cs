using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(TaskWebsite), Schema = DbSchemas.Task)]
    public class TaskWebsite : ITaskable
    {
        [Required]
        public int TaskId { get; set; }

        public virtual ProjectTask Task { get; set; }

        [Required]
        public int WebsiteId { get; set; }

        public virtual Website Website { get; set; }
    }
}
