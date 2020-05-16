using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels
{
    public class AccruedTimeBindingModel
    {
        [Required]
        public DateTime? From { get; set; }
        
        [Required]
        public DateTime? To { get; set; }
        
        [Required]
        public int? TaskId { get; set; }
    }
}
