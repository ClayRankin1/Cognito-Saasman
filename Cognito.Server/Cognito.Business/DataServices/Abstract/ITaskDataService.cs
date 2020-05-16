using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface ITaskDataService : IDataService<ProjectTask, TaskViewModel>
    {
        Task<TaskViewModel[]> GetTasksByProjectId(TaskStatusId status, int? projectId);

        Task<TaskViewModel[]> GetTasksByProjectIds(TaskStatusId status, IEnumerable<int> projectIds);

        Task<TaskViewModel> CompleteTaskAsync(int taskId);

        Task<TaskViewModel> MoveTaskAsync(int taskId, DateTime nextDate);

        Task<TaskViewModel[]> AdvanceTasksAsync();
    }
}
