using Cognito.DataAccess.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cognito.DataAccess.Entities
{
    [DbTable(nameof(RefreshToken), Schema = DbSchemas.User)]
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [MaxLength(64)]
        public string Token { get; set; }

        [Required]
        public DateTime Expiration { get; set; }
    }
}
