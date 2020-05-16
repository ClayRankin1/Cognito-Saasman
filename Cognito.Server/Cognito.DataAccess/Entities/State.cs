using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(State), Schema = DbSchemas.Lookup)]
    public class State : LookupBase<StateId>
    {
    }
}
