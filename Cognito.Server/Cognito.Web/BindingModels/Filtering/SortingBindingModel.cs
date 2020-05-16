using Cognito.Shared.Filtering;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Filtering
{
    public class SortingBindingModel
    {
        [Required]
        public string Field { get; set; }

        [Required]
        public SortDirection? Direction { get; set; }
    }
}
