namespace Cognito.Business.ViewModels
{
    public class DomainLicenseViewModel
    {
        public int LicenseId { get; set; }

        public int DomainId { get; set; }

        public LicenseViewModel License { get; set; }

        public int Licenses { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }
    }
}
