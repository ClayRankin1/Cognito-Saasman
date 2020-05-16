using System.Collections.Generic;

namespace Cognito.Web.ViewModels.Error
{
    public class ErrorWrapperViewModel
    {
        public IList<ErrorViewModel> Toasts { get; } = new List<ErrorViewModel>();

        public IList<ErrorViewModel> Errors { get; } = new List<ErrorViewModel>();
    }
}
