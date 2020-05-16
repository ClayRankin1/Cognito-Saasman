using Cognito.DataAccess.Entities;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public interface ITaskRepository : ICrudRepository<ProjectTask>
    {
        Task AddContactAsync(int taskId, int contactId);

        Task AddDocumentAsync(int taskId, int documentId);
    }
}
