namespace Cognito.Business.ViewModels
{
    public class PointViewModel : IdentityViewModel
    {
        public int DisplayOrder { get; set; }

        public int ProjectId { get; set; }

        public string Text { get; set; }

        public int? ParentId { get; set; }

        public int Count { get; set; }

        public string Label { get; set; }
    }
}
