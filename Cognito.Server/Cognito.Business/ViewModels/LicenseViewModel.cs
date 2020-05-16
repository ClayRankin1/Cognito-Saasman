namespace Cognito.Business.ViewModels
{
    public class LicenseViewModel : IdentityViewModel
    {
        public decimal Price { get; set; }

        public int LicenseTypeId { get; set; }
    }
}
