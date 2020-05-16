using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels.Project;
using Cognito.Web.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    public class ProjectsController : CrudControllerBase<Project, ProjectBindingModel, ProjectViewModel, IProjectDataService>
    {
        private readonly ITaskDataService _taskDataService;
        private readonly IDetailDataService _detailDataService;

        public ProjectsController(
            IMapper mapper,
            IProjectDataService dataService,
            ITaskDataService taskDataService,
            IDetailDataService detailDataService) : base(mapper, dataService)
        {
            _taskDataService = taskDataService;
            _detailDataService = detailDataService;
        }

        [HttpGet("{id}/team")]
        public async Task<IActionResult> GetProjectTeamById(int id)
        {
            var teammates = await _dataService.GetProjectTeamAsync(id);
            return Ok(teammates);
        }

        [HttpGet("{id}/linked-details")]
        public async Task<IActionResult> GetLinkedDetails(int id)
        {
            var pointDetails = await _detailDataService.GetLinkedDetailsAsync(id);
            return Ok(pointDetails);
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks(TaskStatusId status, int? projectId)
        {
            var tasks = await _taskDataService.GetTasksByProjectId(status, projectId);
            return Ok(tasks);
        }
    }
}
