using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Project
{
    public class ProjectBindingModel
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(25)]
        public string Nickname { get; set; }

        [MaxLength(30)]
        public string ProjectNo { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsBillable { get; set; }

        [Required]
        public int? OwnerId { get; set; }

        public int? ProxyId { get; set; }

        [MaxLength(30)]
        public string ClientNo { get; set; }

        [Required]
        public int? DomainId { get; set; }

        public List<UserProjectBindingModel> Users { get; set; }
    }
}
