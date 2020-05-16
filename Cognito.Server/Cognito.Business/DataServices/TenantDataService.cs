using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class TenantDataService : DataServiceBase<Tenant, TenantViewModel, ITenantRepository>, ITenantDataService
    {
        private readonly IPermissionsService _permissionsService;

        public TenantDataService(
            IMapper mapper,
            ITenantRepository tenantRepository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IPermissionsService permissionsService) : base(mapper, tenantRepository, dateTimeProvider, currentUserService)
        {
            _permissionsService = permissionsService;
        }

        public override async Task<TenantViewModel[]> GetAllAsync()
        {
            var isSysAdmin = _permissionsService.IsInRole(UserRoles.SysAdmin);

            var tenants = await _repository
                .GetAll()
                .Select(t => new Tenant
                {
                    Id = t.Id,
                    Address = t.Address,
                    ContactName = t.ContactName,
                    Email = t.Email,
                    Phone = t.Phone,
                    CompanyName = t.CompanyName,
                    TenantName = t.TenantName,
                    Domains = t.Domains.Select(d => new Domain
                    {
                        UserDomains = d.UserDomains
                            .Where(ud => ud.RoleId == (int)UserRoles.TenantAdmin && (isSysAdmin || ud.UserId == _currentUserService.UserId))
                            .Select(ud => new UserDomain
                            {
                                User = new User
                                {
                                    Email = ud.User.Email
                                }
                            }).ToList()
                    }).ToList()
                })
                .OrderBy(t => t.TenantName ?? t.CompanyName)
                .ToListAsync();

            if (!isSysAdmin)
            {
                //Appling this filter directly in the query made it take 25 seconds to run
                //TODO: optimize it, maybe write some raw SQL and use dapper?
                tenants.RemoveAll(t => !t.Domains.Any(d => d.UserDomains.Any()));
            }

            return _mapper.Map<TenantViewModel[]>(tenants);
        }

        public override async Task<TenantViewModel> UpdateAsync(Tenant entity)
        {
            FillAuditableProperties(entity, isCreatingNewEntity: false);

            if (_permissionsService.IsInRole(UserRoles.SysAdmin) || await _permissionsService.IsAdminForTenant(entity))
            {
                entity.Address.Id = await GetTenantAddressId(entity);
                return await base.UpdateAsync(entity);
            }

            return _mapper.Map<TenantViewModel>(entity);
        }


        private async Task<int> GetTenantAddressId(Tenant tenant)
        {
            return await _repository.GetAll()
                .Where(t => t.Id == tenant.Id)
                .Select(t => t.AddressId).SingleOrDefaultAsync();
        }
    }
}
