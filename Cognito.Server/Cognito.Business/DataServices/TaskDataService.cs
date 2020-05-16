using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.DataAccess.Services.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class TaskDataService : DataServiceBase<ProjectTask, TaskViewModel, ITaskRepository>, ITaskDataService
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IStoreProcedureRunner _storeProcedureRunner;

        public TaskDataService(
            IMapper mapper,
            ITaskRepository taskRepository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IPermissionsService permissionsService,
            IStoreProcedureRunner storeProcedureRunner) : base(mapper, taskRepository, dateTimeProvider, currentUserService)
        {
            _permissionsService = permissionsService;
            _storeProcedureRunner = storeProcedureRunner;
        }

        public Task<TaskViewModel[]> GetTasksByProjectId(TaskStatusId status, int? projectId)
        {
            var query = _repository
                .GetAll()
                .Include(t => t.Subtasks)
                .Where(t => t.TaskStatusId == status);

            if (projectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == projectId.Value);
            }

            return query
                .OrderBy(t => t.TimeId)
                .ProjectTo<TaskViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<TaskViewModel[]> GetTasksByProjectIds(TaskStatusId status, IEnumerable<int> projectIds)
        {
            await _permissionsService.EnsureProjectsAccessAsync(projectIds);

            return await _repository
                .GetAll()
                .Include(t => t.Subtasks)
                .Where(t => t.TaskStatusId == status && projectIds.Contains(t.ProjectId))
                .OrderBy(t => t.TimeId)
                .ProjectTo<TaskViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public override async Task<TaskViewModel> UpdateAsync(ProjectTask entity)
        {
            // Merge subtasks
            await _storeProcedureRunner.ExecuteAsync(StoredProcedures.MergeSubtasks, new 
            {
                TaskId = entity.Id,
                UserIds = string.Join(",", entity.Subtasks.Select(t => t.UserId))
            });

            // Clear the subtasks collection so EF does not need to update it
            entity.Subtasks.Clear();

            return await base.UpdateAsync(entity);
        }

        public async Task<TaskViewModel> CompleteTaskAsync(int taskId)
        {
            var task = await _repository.GetByIdAsync(taskId);
            task.TaskStatusId = TaskStatusId.Complete;

            return await UpdateTaskAsync(task);
        }

        public async Task<TaskViewModel> MoveTaskAsync(int taskId, DateTime nextDate)
        {
            var task = await _repository.GetByIdAsync(taskId);
            task.NextDate = nextDate;

            return await UpdateTaskAsync(task);
        }

        public async Task<TaskViewModel[]> AdvanceTasksAsync()
        {
            await _repository
                .GetAll()
                .Where(task =>
                    task.CreatedByUserId == _currentUserService.UserId &&
                    task.NextDate < _dateTimeProvider.UtcNow &&
                    task.TaskStatusId == TaskStatusId.Pending
                )
                .BatchUpdateAsync(task => new ProjectTask
                {
                    NextDate = _dateTimeProvider.UtcNow
                });

            return await _repository
                .GetAll()
                .ProjectTo<TaskViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        private async Task<TaskViewModel> UpdateTaskAsync(ProjectTask task)
        {
            _repository.Update(task);
            
            await _repository.SaveAllAsync();

            return _mapper.Map<TaskViewModel>(task);
        }
    }
}
