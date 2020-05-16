namespace Cognito.Web.BindingModels.Detail
{
    public class DetailExtractionBindingModel
    {
        public string Extracted { get; set; }

        public int? ActId { get; set; }
        
        public int? MatterId { get; set; }
        
        public string Source { get; set; }
        
        public int? OwnerId { get; set; }
        
        public int? DomainId { get; set; }
    }
}