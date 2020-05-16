using System.Collections.Generic;

namespace Cognito.DataAccess.Filtering
{
    public class DataFilter
    {
        public PropertyFilter Filter { get; set; }

        public IEnumerable<Sorting> Sorts { get; set; }

        public Paging Paging { get; set; }
    }
}
