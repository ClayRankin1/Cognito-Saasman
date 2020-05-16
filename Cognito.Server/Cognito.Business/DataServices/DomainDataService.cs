using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.Business.ViewModels.Task;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class DomainDataService : DataServiceBase<Domain, DomainViewModel, IDomainRepository>, IDomainDataService
    {
        private readonly IPermissionsService _permissionsService;

        public DomainDataService(
            IMapper mapper, 
            IDomainRepository repository, 
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IPermissionsService permissionsService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
            _permissionsService = permissionsService;
        }

        public Task<TeamMemberViewModel[]> GetDomainTeamAsync(int domainId)
        {
            return _repository
                .GetAll()
                .Where(d => d.Id == domainId)
                .SelectMany(d => d.UserDomains)
                .Select(ud => ud.User)
                .ProjectTo<TeamMemberViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<DomainViewModel>> GetDomainsByTenantIdAsync(int tenantId)
        {
            return await _repository.GetDomainsByTenantId(tenantId)
                .OrderBy(d => d.Name)
                .ProjectTo<DomainViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<DomainViewModel>> GetAdminDomainsAsync()
        {
            var domains = await _repository
                .GetAdminDomains()
                .Where(d => d.UserDomains.Any(ud => ud.UserId == _currentUserService.UserId && ud.RoleId == (int)UserRoles.DomainAdmin) ||
                            d.Projects.Any(p => p.OwnerId == _currentUserService.UserId || p.ProxyId == _currentUserService.UserId))
                .Select(d => new Domain
                {
                    Id = d.Id,
                    Name = d.Name,
                    Tenant = new Tenant
                    {
                        TenantName = d.Tenant.TenantName,
                        CompanyName = d.Tenant.CompanyName
                    }
                }).ToListAsync();

            return _mapper.Map<IEnumerable<DomainViewModel>>(domains);
        }
    }
}
