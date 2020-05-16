using Cognito.Shared.Filtering;

namespace Cognito.DataAccess.Filtering
{
    public class Sorting
    {
        public string Field { get; set; }

        public SortDirection Direction { get; set; }
    }
}
