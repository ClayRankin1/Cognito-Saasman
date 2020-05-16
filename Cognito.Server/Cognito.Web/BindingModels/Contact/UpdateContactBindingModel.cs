using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.BindingModels.Contact
{
    public class UpdateContactBindingModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string FormalName { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Entity { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Region { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(25)]
        public string Phone { get; set; }

        [MaxLength(128)]
        public string Email { get; set; }

        public string Notes { get; set; }

        [MaxLength(10)]
        public string KeyName { get; set; }

        [MaxLength(50)]
        public string ContactType { get; set; }
    }
}
