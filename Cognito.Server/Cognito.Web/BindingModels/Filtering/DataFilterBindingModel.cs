using System.Collections.Generic;

namespace Cognito.Web.BindingModels.Filtering
{
    public class DataFilterBindingModel
    {
        public PropertyFilterBindingModel Filter { get; set; }

        public IEnumerable<SortingBindingModel> Sorts { get; set; }

        public PagingBindingModel Paging { get; set; }
    }
}
