using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(DetailTypeSourceType), Schema = DbSchemas.Lookup)]
    public class DetailTypeSourceType : LookupBase<DetailTypeSourceTypeId>
    {

    }
}
