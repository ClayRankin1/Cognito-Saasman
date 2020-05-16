using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class Message : EntityBase
    {
        [Required]
        public int SenderId { get; set; }

        public virtual User Sender { get; set; }

        [Required]
        public int RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime DateSent { get; set; }

        public bool IsSenderDeleted { get; set; }

        public bool IsRecipientDeleted { get; set; }
    }
}
