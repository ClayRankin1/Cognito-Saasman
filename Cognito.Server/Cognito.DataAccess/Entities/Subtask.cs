using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(Subtask), Schema = DbSchemas.Task)]
    public class Subtask : EntityBase
    {
        public int TaskId { get; set; }

        public virtual ProjectTask Task { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
