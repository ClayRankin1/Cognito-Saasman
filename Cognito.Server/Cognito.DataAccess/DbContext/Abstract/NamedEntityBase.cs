using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public abstract class NamedEntityBase : EntityBase, INamed
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
