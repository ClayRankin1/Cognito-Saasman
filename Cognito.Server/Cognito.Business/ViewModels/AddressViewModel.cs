namespace Cognito.Business.ViewModels
{
    public class AddressViewModel
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public int StateId { get; set; }

        public StateViewModel State { get; set; }
    }
}
