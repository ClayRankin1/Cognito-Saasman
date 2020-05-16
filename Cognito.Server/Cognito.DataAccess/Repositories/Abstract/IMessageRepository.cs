using Cognito.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public interface IMessageRepository : ICrudRepository<Message>
    {
        // Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);

        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}
