using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels.Common;
using Cognito.Web.BindingModels.Detail;
using Cognito.Web.Controllers.Abstract;
using Cognito.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    public class DetailsController : CrudControllerBase<Detail, DetailBindingModel, DetailViewModel, IDetailDataService>
    {
        public DetailsController(
            IMapper mapper,
            IDetailDataService dataService) : base(mapper, dataService)
        {
        }

        [TransactionScope]
        [HttpPut(nameof(Split))]
        public async Task<IActionResult> Split(MergeSplitItemsBindingModel model)
        {
            await _dataService.SplitAsync(model.DetailIds);
            return Ok();
        }

        [TransactionScope]
        [HttpPut(nameof(Merge))]
        public async Task<IActionResult> Merge(MergeSplitItemsBindingModel model)
        {
            await _dataService.MergeAsync(model.DetailIds);
            return Ok();
        }

        /// <summary>
        /// Copies Detail into specified Task.
        /// </summary>
        /// <param name="detailId">The Detail Id to copy.</param>
        /// <param name="taskId">The target Task Id to copy the Detail into.</param>
        [TransactionScope]
        [HttpPost("{detailId}/copy/{taskId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Copy(int detailId, int taskId)
        {
            var detail = await _dataService.CopyDetailAsync(detailId, taskId);
            return CreatedAtAction(nameof(Get), new { id = detail.Id }, detail);
        }

        [HttpPost(nameof(Extraction))]
        public Task<IActionResult> Extraction(DetailExtractionBindingModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes several Details in bulk.
        /// </summary>
        [HttpDelete(nameof(Bulk))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Bulk(IdsBindingModel model)
        {
            await _dataService.BulkDeleteDetailsAsync(model.Ids);
            return NoContent();
        }
    }
}
