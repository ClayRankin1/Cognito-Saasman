using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(LicenseType), Schema = DbSchemas.Lookup)]
    public class LicenseType : LookupBase<LicenseTypeId>
    {
    }
}
