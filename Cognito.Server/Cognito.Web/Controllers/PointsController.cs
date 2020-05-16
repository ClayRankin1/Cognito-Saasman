using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels.Point;
using Cognito.Web.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : CrudControllerBase<Point, CreatePointBindingModel, UpdatePointBindingModel, PointViewModel, IPointDataService>
    {
        public PointsController(IMapper mapper, IPointDataService dataService): base(mapper, dataService)
        {
        }

        [HttpGet("linked")]
        public async Task<IActionResult> GetPointDetails([FromQuery]int[] detailIds)
        {
            var points = await _dataService.GetPointDetailsByDetailIdsAsync(detailIds);
            return Ok(points);
        }

        // TODO: How can we override inherited GET request to map to our custom impl?
        [HttpGet("project")]
        public async Task<IActionResult> GetByProjectId([FromQuery]int projectId)
        {
            var points = await _dataService.GetByProjectIdAsync(projectId);
            return Ok(points);
        }
        
        [HttpPost("{id}/link")]
        public async Task<IActionResult> AddPointDetails(int id, LinkPointToDetailsBindingModel model)
        {
            await _dataService.AddPointDetailsAsync(id, model.DetailIds);
            return NoContent();
        }

        [HttpPost("{id}/reorder")]
        public async Task<IActionResult> ReorderPoint(int id, ReorderPointBindingModel model)
        {
            var point = _mapper.Map<Point>(model);
            point.Id = id;
            await _dataService.ReorderAsync(point);
            return NoContent();
        }
    }
}