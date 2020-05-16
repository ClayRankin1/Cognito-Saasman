using System;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public abstract class DateAuditableEntityBase : EntityBase, IDateAuditable
    {
        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
