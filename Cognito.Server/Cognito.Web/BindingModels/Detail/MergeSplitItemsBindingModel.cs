using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Detail
{
    public class MergeSplitItemsBindingModel
    {
        [Required]
        public int[] DetailIds { get; set; }
    }
}
