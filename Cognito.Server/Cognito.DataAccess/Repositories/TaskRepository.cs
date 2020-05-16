using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Repositories
{
    public class TaskRepository : CrudRepository<ProjectTask>, ITaskRepository
    {
        public TaskRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }

        protected override IQueryable<ProjectTask> GetAllInternal()
        {
            return _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.TaskType)
                .Include(t => t.CreatedByUser)
                .Include(t => t.Details)
                .Include(t => t.AccruedTimes)
                .Include(t => t.Subtasks)
                .Where(t =>
                    t.CreatedByUserId == _currentUserService.UserId ||
                    t.Project.Users.Any(up => up.UserId == _currentUserService.UserId)
                )
                .OrderBy(t => t.TimeId);
        }

        public Task AddContactAsync(int taskId, int contactId)
        {
            _context.TaskContacts.Add(new TaskContact
            {
                TaskId = taskId,
                ContactId = contactId
            });

            return _context.SaveChangesAsync();
        }

        public Task AddDocumentAsync(int taskId, int documentId)
        {
            _context.TaskDocuments.Add(new TaskDocument
            {
                TaskId = taskId,
                DocumentId = documentId
            });

            return _context.SaveChangesAsync();
        }
    }
}
