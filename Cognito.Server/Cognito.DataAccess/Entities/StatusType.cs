using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(StatusType), Schema = DbSchemas.Lookup)]
    public class StatusType : LookupBase<int>
    {
        [Required]
        public int DisplayOrder { get; set; }
    }
}
