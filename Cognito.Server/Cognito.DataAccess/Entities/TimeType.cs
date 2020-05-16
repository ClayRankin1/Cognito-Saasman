using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(TimeType), Schema = DbSchemas.Lookup)]
    public class TimeType : LookupBase<TimeTypeId>
    {
        public virtual ICollection<Time> Times { get; set; }
    }
}
