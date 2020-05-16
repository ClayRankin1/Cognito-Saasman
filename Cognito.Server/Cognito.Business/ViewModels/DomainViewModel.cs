using Cognito.DataAccess.Entities;
using System.Collections.Generic;

namespace Cognito.Business.ViewModels
{
    public class DomainViewModel : IdentityViewModel
    {
        public string Name { get; set; }

        public int? AdminUserId { get; set; }

        public TenantViewModel Tenant { get; set; }

        public int? TenantId { get; set; }

        public List<DomainLicenseViewModel> DomainLicenses { get; set; }

        public List<UserDomainViewModel> UserDomains { get; set; }
    }
}
