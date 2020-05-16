using System;

namespace Cognito.Business.ViewModels
{
    public class DetailViewModel : AuditableViewModel
    {
        public string Detail { get; set; }

        public string Source { get; set; }

        public DateTime Added { get; set; }

        public string Subject { get; set; }

        public int? BeginPage { get; set; }

        public int? BeginLine { get; set; }

        public int? EndPage { get; set; }

        public int? EndLine { get; set; }

        public DateTime? Chrono { get; set; }

        public int? DisplayOrder { get; set; }

        public int? DetailTypeId { get; set; }

        public int? TaskId { get; set; }
    }
}
