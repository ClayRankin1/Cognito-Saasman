using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(TaskType), Schema = DbSchemas.Lookup)]
    public class TaskType : LookupBase<int>, IEntity, IDateAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public bool IsEvent { get; set; }

        public DateTime? ArchivedOn { get; set; }

        public int? DomainId { get; set; }

        public virtual Domain Domain { get; set; }

        public bool IsDeleted { get; set; }
    }
}
