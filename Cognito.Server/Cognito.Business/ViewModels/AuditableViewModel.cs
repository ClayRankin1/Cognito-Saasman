using System;

namespace Cognito.Business.ViewModels
{
    public class AuditableViewModel : IdentityViewModel
    {
        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedByUser { get; set; }

        public int CreatedByUserId { get; set; }

        public int UpdatedByUserId { get; set; }
    }
}
