using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels.Task;
using Cognito.Web.Controllers.Abstract;
using Cognito.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    public class TasksController : CrudControllerBase<ProjectTask, TaskBindingModel, TaskViewModel, ITaskDataService>
    {
        private readonly ITaskService _taskService;
        private readonly IDetailDataService _detailDataService;

        public TasksController(
            IMapper mapper,
            ITaskService taskService,
            ITaskDataService dataService,
            IDetailDataService detailDataService) : base(mapper, dataService)
        {
            _taskService = taskService;
            _detailDataService = detailDataService;
        }

        [HttpGet("{taskId}/details")]
        public async Task<IActionResult> GetDetailsByTaskId(int taskId, [FromQuery] int? detailTypeId)
        {
            var details = await _detailDataService.GetDetailsByTaskIdAsync(taskId, detailTypeId);
            return Ok(details);
        }

        [Obsolete]
        [HttpGet("project")]
        public async Task<IActionResult> GetTasksByProjectId(TaskStatusId status, int? projectId)
        {
            var tasks = await _dataService.GetTasksByProjectId(status, projectId);
            return Ok(tasks);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(TaskViewModel[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        // This is POST by design it is easier to bind array in POST in ASP.NET Core
        // But we can introduce some custom binder to support arrays in QuertyString
        public async Task<IActionResult> GetTasksBulkAsync(GetTasksFilterBindingModel model)
        {
            var tasks = await _dataService.GetTasksByProjectIds(model.Status.Value, model.ProjectIds);
            return Ok(tasks);
        }

        /// <summary>
        /// Copies the Task only.
        /// </summary>
        [TransactionScope]
        [HttpPost("{taskId}/copy")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CopyTask(int taskId)
        {
            await _taskService.CopyTaskAsync(taskId);
            return NoContent();
        }

        /// <summary>
        /// Clones Task including Details, Websites, Documents and Contacts.
        /// </summary>
        [TransactionScope]
        [HttpPost("{taskId}/clone")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CloneTask(int taskId)
        {
            await _taskService.CloneTaskAsync(taskId);
            return NoContent();
        }

        [HttpPut(nameof(Complete))]
        public async Task<IActionResult> Complete(TaskIdBindingModel model)
        {
            var task = await _dataService.CompleteTaskAsync(model.TaskId.Value);
            return Ok(task);
        }

        [HttpPut(nameof(Move))]
        public async Task<IActionResult> Move(MoveTaskBindingModel model)
        {
            var task = await _dataService.MoveTaskAsync(model.TaskId.Value, model.NextDate.Value);
            return Ok(task);
        }

        [HttpPut(nameof(Advance))]
        public async Task<IActionResult> Advance()
        {
            var tasks = await _dataService.AdvanceTasksAsync();
            return Ok(tasks);
        }
    }
}
