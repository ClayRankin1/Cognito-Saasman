using Cognito.DataAccess.DbContext.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class Address : EntityBase
    {
        [Required]
        [MaxLength(250)]
        public string Street { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(10)]
        public string Zip { get; set; }

        [Required]
        public StateId StateId { get; set; }

        public virtual State State { get; set; }
    }
}
