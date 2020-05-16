using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Models;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels.Common;
using Cognito.Web.BindingModels.Website;
using Cognito.Web.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    public class WebsitesController : CrudControllerBase<Website, CreateWebsiteBindingModel, UpdateWebsiteBindingModel, WebsiteViewModel, IWebsiteDataService>
    {
        private readonly IWebsiteService _websiteService;

        public WebsitesController(
            IMapper mapper,
            IWebsiteDataService dataService,
            IWebsiteService websiteService) : base(mapper, dataService)
        {
            _websiteService = websiteService;
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> GetAll() => base.GetAll();

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] LocalFilterBindingModel model)
        {
            var filter = _mapper.Map<LocalFilter>(model);
            return Ok(await _dataService.GetWebsitesAsync(filter));
        }

        public override async Task<IActionResult> Create([FromBody] CreateWebsiteBindingModel model)
        {
            var website = _mapper.Map<Website>(model);

            return Ok(await _websiteService.CreateWebsiteAsync(website, model.TaskId.Value));
        }

        public override async Task<IActionResult> Update(int id, [FromBody] UpdateWebsiteBindingModel model)
        {
            var website = _mapper.Map<Website>(model);

            website.Id = id;

            return Ok(await _websiteService.UpdateWebsiteAsync(website));
        }

        public override async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _websiteService.DeleteWebsiteAsync(id);
            return NoContent();
        }
    }
}
