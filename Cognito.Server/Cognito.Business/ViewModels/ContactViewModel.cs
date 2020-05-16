namespace Cognito.Business.ViewModels
{
    public class ContactViewModel : AuditableViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name { get; set; }

        public string FormalName { get; set; }

        public string Title { get; set; }

        public string Entity { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }

        public string KeyName { get; set; }

        public string ContactType { get; set; }
    }
}
