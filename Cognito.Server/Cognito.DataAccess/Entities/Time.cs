using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(Time), Schema = DbSchemas.Lookup)]
    public class Time : LookupBase<int>
    {
        public TimeTypeId TimeTypeId { get; set; }

        public virtual TimeType TimeType { get; set; }

        public virtual ICollection<ProjectTask> Tasks { get; set; }
    }
}
