using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Shared.Security;
using Cognito.Web.BindingModels;
using Cognito.Web.Controllers.Abstract;
using Cognito.Web.Infrastructure.Attributes;
using Cognito.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    public class TenantsController : CrudControllerBase<Tenant, TenantBindingModel, TenantViewModel, ITenantDataService>
    {
        private readonly IDomainDataService _domainDataService;

        public TenantsController(
            IMapper mapper,
            ITenantDataService dataService,
            IDomainDataService domainDataService) : base(mapper, dataService)
        {
            _domainDataService = domainDataService;
        }

        public override Task<IActionResult> GetAll() => base.GetAll();

        [TransactionScope]
        [AuthorizeRoles(UserRoles.SysAdmin)]
        public override Task<IActionResult> Create([FromBody] TenantBindingModel model) => base.Create(model);

        [TransactionScope]
        public override Task<IActionResult> Update(int id, [FromBody] TenantBindingModel model) => base.Update(id, model);

        [AuthorizeRoles(UserRoles.SysAdmin)]
        public override Task<IActionResult> Delete([FromRoute] int id) => base.Delete(id);

        [HttpGet("{tenantId}/domains")]
        public async Task<IActionResult> GetDomainsByTenantId(int tenantId) => Ok(await _domainDataService.GetDomainsByTenantIdAsync(tenantId));
    }
}
