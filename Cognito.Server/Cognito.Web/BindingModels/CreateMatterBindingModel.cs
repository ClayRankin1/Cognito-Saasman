using System;

namespace Cognito.Web.BindingModels
{
    public class CreateMatterBindingModel
    {
        // TODO: Find out what is required!!!
        public bool? Flag { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string MatterNo { get; set; }
        public string ClientNo { get; set; }
        public int? OwnerId { get; set; }
        public string Type { get; set; }
        public bool? IsPrivate { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? Archived { get; set; }
        public bool? IsArchived { get; set; }
        public int? DomainId { get; set; }
    }
}