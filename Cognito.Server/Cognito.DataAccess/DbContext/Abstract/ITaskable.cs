using Cognito.DataAccess.Entities;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public interface ITaskable
    {
        int TaskId { get; set; }

        ProjectTask Task { get; set; }
    }
}
