using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class Project : AuditableEntityBase
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(25)]
        public string Nickname { get; set; }

        [MaxLength(30)]
        public string ProjectNo { get; set; }

        [MaxLength(30)]
        public string ClientNo { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime? ArchivedOn { get; set; }

        public bool IsBillable { get; set; }

        public int DomainId { get; set; }

        public virtual Domain Domain { get; set; }

        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }
        
        public int? ProxyId { get; set; }
        public virtual User Proxy { get; set; }

        public virtual ICollection<ProjectUser> Users { get; set; }

        public virtual ICollection<ProjectTask> Tasks { get; set; }

        public virtual ICollection<Point> Points { get; set; }
    }
}
