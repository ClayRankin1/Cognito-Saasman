using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Filtering;
using Cognito.Web.BindingModels.Filtering;
using Cognito.Web.Infrastructure.Attributes;
using Cognito.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers.Abstract
{
    [CognitoApi]
    [ApiController]
    [CognitoProducesJson]
    public abstract class CrudControllerBase<TEntity, TCreateInput, TUpdateInput, TOutput, TDataService> : ControllerBase
        where TEntity : class, IEntity, new()
        where TCreateInput : class
        where TUpdateInput : class
        where TOutput : IdentityViewModel, new()
        where TDataService : class, IDataService<TEntity, TOutput>
    {
        protected readonly IMapper _mapper;
        protected readonly TDataService _dataService;

        protected CrudControllerBase(
            IMapper mapper,
            TDataService dataService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [HttpGet, Route("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            return Ok(await _dataService.GetAsync(id));
        }

        [HttpGet, Route("")]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(await _dataService.GetAllAsync());
        }

        // POST by design, it might be easier to use POST data then the query string
        [HttpPost, Route("filter")]
        public virtual async Task<IActionResult> GetFilteredData(DataFilterBindingModel model)
        {
            var filter = _mapper.Map<DataFilter>(model);
            var response = await _dataService.FilterAsync(filter);
            return Ok(response);
        }

        [TransactionScope]
        [HttpPost, Route("")]
        public virtual async Task<IActionResult> Create([FromBody]TCreateInput model)
        {
            var entity = _mapper.Map<TEntity>(model);
            var result = await _dataService.CreateAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [TransactionScope]
        [HttpPut, Route("{id}")]
        public virtual async Task<IActionResult> Update(int id, [FromBody]TUpdateInput model)
        {
            var mappedModel = _mapper.Map<TEntity>(model);
            mappedModel.Id = id;
            await _dataService.UpdateAsync(mappedModel);
            return NoContent();
        }

        [TransactionScope]
        [HttpDelete, Route("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _dataService.DeleteAsync(id);
            return NoContent();
        }
    }

    public abstract class CrudControllerBase<TEntity, TInput, TOutput, TDataService> : CrudControllerBase<TEntity, TInput, TInput, TOutput, TDataService>
        where TEntity : class, IEntity, new()
        where TInput : class
        where TOutput : IdentityViewModel, new()
        where TDataService : class, IDataService<TEntity, TOutput>
    {
        protected CrudControllerBase(IMapper mapper, TDataService dataService) : base(mapper, dataService)
        {
        }
    }
}
