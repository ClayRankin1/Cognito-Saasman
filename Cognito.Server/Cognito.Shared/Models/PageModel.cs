namespace Cognito.Shared.Models
{
    public class PageModel
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string SortField { get; set; }

        public string SortOrder { get; set; }
    }
}
