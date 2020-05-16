using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Repositories
{
    public class MessageRepository : CrudRepository<Message>, IMessageRepository
    {
        public MessageRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }

        //public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        //{
        //    var messages = _context.Messages.Include(u => u.Sender).Include(u => u.Recipient).AsQueryable();

        //    switch (messageParams.MessageContainer)
        //    {
        //        case "Inbox":
        //            messages = messages.Where(u => u.RecipientId == messageParams.UserId);
        //            break;
        //        case "Outbox":
        //            messages = messages.Where(u => u.SenderId == messageParams.UserId);
        //            break;
        //        default:
        //            messages = messages.Where(u => u.RecipientId == messageParams.UserId && !u.IsRead);
        //            break;
        //    }

        //    messages = messages.OrderByDescending(d => d.MessageSent);

        //    return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        //}

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .Where(m => m.RecipientId == userId || m.RecipientId == recipientId && m.SenderId == userId)
                .OrderByDescending(m => m.DateSent)
                .ToListAsync();

            return messages;
        }
    }
}
