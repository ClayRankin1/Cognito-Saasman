using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels.Domain;
using Cognito.Web.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    public class DomainsController : CrudControllerBase<Domain, CreateDomainBindingModel, UpdateDomainBindingModel, DomainViewModel, IDomainDataService>
    {
        private readonly IProjectDataService _projectDataService;
        private readonly IUserDataService _userDataService;

        public DomainsController(
            IMapper mapper,
            IDomainDataService dataService,
            IProjectDataService projectDataService,
            IUserDataService userDataService) : base(mapper, dataService)
        {
            _projectDataService = projectDataService;
            _userDataService = userDataService;
        }

        [HttpGet("{id}/team")]
        public async Task<IActionResult> GetDomainTeam(int id)
        {
            var teammates = await _dataService.GetDomainTeamAsync(id);
            return Ok(teammates);
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminDomains()
        {
            var domains = await _dataService.GetAdminDomainsAsync();
            return Ok(domains);
        }

        [HttpGet("{id}/projects")]
        public async Task<IActionResult> GetDomainProjects(int id)
        {
            var projects = await _projectDataService.GetProjectsByDomainIdAsync(id);
            return Ok(projects);
        }

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetDomainUsers(int id)
        {
            var users = await _userDataService.GetUsersByDomainIdAsync(id);
            return Ok(users);
        }
    }
}