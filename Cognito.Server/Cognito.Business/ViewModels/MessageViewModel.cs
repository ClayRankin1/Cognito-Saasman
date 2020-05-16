using System;

namespace Cognito.Business.ViewModels
{
    public class MessageViewModel : IdentityViewModel
    {
        public int SenderId { get; set; }

        public string SenderUserName { get; set; }

        public int RecipientId { get; set; }

        public string RecipientUserName { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime DateSent { get; set; }
    }
}
