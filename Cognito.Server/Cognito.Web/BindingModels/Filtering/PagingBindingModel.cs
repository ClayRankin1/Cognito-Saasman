using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Filtering
{
    public class PagingBindingModel
    {
        [Required]
        public int? PageNumber { get; set; }

        [Required]
        public int? PageSize { get; set; }
    }
}
