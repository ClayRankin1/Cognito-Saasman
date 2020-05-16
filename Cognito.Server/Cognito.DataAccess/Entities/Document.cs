using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(Document), Schema = DbSchemas.Detail)]
    public class Document : AuditableEntityBase
    {
        [Required]
        [MaxLength(255)]
        public string Key { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        // TODO: Always better to have some limit
        [MaxLength(1000)]
        public string Description { get; set; }

        public DocumentStatusId DocumentStatusId { get; set; }

        public virtual DocumentStatus DocumentStatus { get; set; }

        public virtual ICollection<TaskDocument> TaskDocuments { get; set; } = new HashSet<TaskDocument>();
    }
}
