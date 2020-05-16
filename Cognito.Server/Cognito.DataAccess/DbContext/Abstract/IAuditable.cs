using Cognito.DataAccess.Entities;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public interface IAuditable : IDateAuditable
    {
        public int CreatedByUserId { get; set; }

        User CreatedByUser { get; set; }

        public int UpdatedByUserId { get; set; }

        User UpdatedByUser { get; set; }
    }
}
