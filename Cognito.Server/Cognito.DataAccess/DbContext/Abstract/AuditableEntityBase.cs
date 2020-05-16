using Cognito.DataAccess.Entities;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public abstract class AuditableEntityBase : DateAuditableEntityBase, IAuditable
    {
        public int CreatedByUserId { get; set; }

        public User CreatedByUser { get; set; }

        public int UpdatedByUserId { get; set; }

        public User UpdatedByUser { get; set; }
    }
}
