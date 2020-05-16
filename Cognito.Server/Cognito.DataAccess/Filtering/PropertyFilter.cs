using Cognito.Shared.Filtering;
using System.Collections.Generic;

namespace Cognito.DataAccess.Filtering
{
    public class PropertyFilter
    {
        public string Field { get; set; }

        public PropertyFilterOperator Operator { get; set; }

        public object Value { get; set; }

        public LogicOperation? Logic { get; set; }

        public IEnumerable<PropertyFilter> Filters { get; set; }
    }
}
