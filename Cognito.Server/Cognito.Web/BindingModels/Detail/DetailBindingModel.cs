using Cognito.DataAccess.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Detail
{
    public class DetailBindingModel
    {
        [Required]
        public string Detail { get; set; }

        public string Source { get; set; }

        public string Subject { get; set; }

        public DateTime? Added { get; set; }

        public int? BeginPage { get; set; }

        public int? BeginLine { get; set; }

        public int? EndPage { get; set; }

        public int? EndLine { get; set; }

        public DateTime? Chrono { get; set; }

        public int? DisplayOrder { get; set; }

        [Required]
        public DetailTypeId? DetailTypeId { get; set; }

        [Required]
        public int? TaskId { get; set; }
    }
}
