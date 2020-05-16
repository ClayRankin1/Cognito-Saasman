namespace Cognito.Business.ViewModels
{
    public class TenantViewModel : IdentityViewModel
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }

        public AddressViewModel Address { get; set; }

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string TenantName { get; set; }

        public string TenantAdminEmails { get; set; }
    }
}
