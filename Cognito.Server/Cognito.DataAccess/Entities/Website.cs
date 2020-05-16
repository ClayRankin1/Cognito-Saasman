using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(Website), Schema = DbSchemas.Dbo)]
    public class Website : EntityBase
    {
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Url { get; set; }

        public virtual ICollection<TaskWebsite> TaskWebsites { get; set; } = new HashSet<TaskWebsite>();
    }
}
