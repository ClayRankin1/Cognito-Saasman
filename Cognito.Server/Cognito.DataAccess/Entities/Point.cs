using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class Point : AuditableEntityBase
    {
        [Required]
        public int DisplayOrder { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [Required]
        public string Text { get; set; }

        public int? ParentId { get; set; }

        public virtual Point Parent { get; set; }

        [Required]
        public int Count { get; set; }
        
        public string Label { get; set; }

        public virtual ICollection<PointDetail> PointDetails { get; set; }

        public virtual ICollection<Point> Children { get; } = new HashSet<Point>();
    }
}
