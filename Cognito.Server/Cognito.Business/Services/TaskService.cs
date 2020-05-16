using Cognito.Business.Services.Abstract;
using Cognito.DataAccess;
using Cognito.DataAccess.Services.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using System.Threading.Tasks;

namespace Cognito.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPermissionsService _permissionsService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStoreProcedureRunner _storeProcedureRunner;

        public TaskService(
            IDateTimeProvider dateTimeProvider,
            IPermissionsService permissionsService,
            ICurrentUserService currentUserService,
            IStoreProcedureRunner storeProcedureRunner)
        {
            _dateTimeProvider = dateTimeProvider;
            _permissionsService = permissionsService;
            _currentUserService = currentUserService;
            _storeProcedureRunner = storeProcedureRunner;
        }

        public async Task CopyTaskAsync(int taskId)
        {
            await _permissionsService.EnsureTaskAccessAsync(taskId);
            await CopyTaskInternalAsync(taskId, false);
        }

        public async Task CloneTaskAsync(int taskId)
        {
            await _permissionsService.EnsureTaskAccessAsync(taskId);
            await CopyTaskInternalAsync(taskId, true);
        }

        private Task<int> CopyTaskInternalAsync(int taskId, bool isFullCopyMode) => _storeProcedureRunner.ExecuteAsync(StoredProcedures.CopyTask, new
        {
            TaskId = taskId,
            IsCopyRelatedData = isFullCopyMode,
            UpdatedByUserId = _currentUserService.UserId,
            DateUpdated = _dateTimeProvider.UtcNow
        });
    }
}
