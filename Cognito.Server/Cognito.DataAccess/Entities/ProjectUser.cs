using Cognito.DataAccess.DbContext.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cognito.DataAccess.Entities
{
    public class ProjectUser: DeletableEntityBase
    {
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        [NotMapped]
        public int PendingTasks { get; set; }
    }
}
