using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(TaskStatus), Schema = DbSchemas.Lookup)]
    public class TaskStatus : LookupBase<TaskStatusId>
    {
    }
}
