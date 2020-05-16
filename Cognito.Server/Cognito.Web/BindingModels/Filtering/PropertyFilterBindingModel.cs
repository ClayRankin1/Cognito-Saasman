using Cognito.Shared.Filtering;
using System.Collections.Generic;

namespace Cognito.Web.BindingModels.Filtering
{
    public class PropertyFilterBindingModel
    {
        public string Field { get; set; }

        public PropertyFilterOperator Operator { get; set; }

        public object Value { get; set; }

        public LogicOperation Logic { get; set; }

        public IEnumerable<PropertyFilterBindingModel> Filters { get; set; }
    }
}
