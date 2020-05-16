using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(DocumentStatus), Schema = DbSchemas.Lookup)]
    public class DocumentStatus : LookupBase<DocumentStatusId>
    {
    }
}
