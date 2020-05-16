using System;

namespace Cognito.Business.ViewModels
{
    public class AccruedTimeViewModel : IdentityViewModel
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int TaskId { get; set; }

        public int UserId { get; set; }

        public decimal Total { get; set; }
    }
}
