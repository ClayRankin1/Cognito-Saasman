using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class RelationBindingModel
    {
        [Required]
        public virtual int? DomainId { get; set; }

        [Required]
        public virtual int? ProjectId { get; set; }

        [Required]
        public virtual int? TaskId { get; set; }
    }
}
