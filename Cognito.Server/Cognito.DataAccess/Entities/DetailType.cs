using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(DetailType), Schema = DbSchemas.Lookup)]
    public class DetailType : LookupBase<DetailTypeId>
    {
        public DetailTypeSourceTypeId DetailTypeSourceTypeId { get; set; }

        public DetailTypeSourceType DetailTypeSourceType { get; set; }
    }
}
