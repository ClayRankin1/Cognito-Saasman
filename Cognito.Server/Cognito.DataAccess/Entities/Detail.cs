using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(Detail), Schema = DbSchemas.Detail)]
    public class Detail : AuditableEntityBase
    {
        [Required]
        public string Body { get; set; }

        [MaxLength(250)]
        public string Subject { get; set; }

        [MaxLength(250)]
        public string Source { get; set; }

        /// <summary>
        /// Represents the Id column reference of e.g. Document, Website, or Contact.
        /// </summary>
        public int? SourceId { get; set; }

        public int? BeginPage { get; set; }

        public int? BeginLine { get; set; }

        public int? EndPage { get; set; }

        public int? EndLine { get; set; }

        public DateTime? Chrono { get; set; }

        public int? DisplayOrder { get; set; }

        [Required]
        public DetailTypeId DetailTypeId { get; set; }

        public virtual DetailType DetailType { get; set; }

        [Required]
        public int TaskId { get; set; }

        public virtual ProjectTask Task { get; set; }

        public virtual ICollection<PointDetail> PointDetails { get; set; }
    }
}
