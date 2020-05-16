using Cognito.Business.ViewModels;
using Cognito.Business.ViewModels.Task;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IDomainDataService : IDataService<DataAccess.Entities.Domain, DomainViewModel>
    {
        Task<IEnumerable<DomainViewModel>> GetAdminDomainsAsync();
        Task<IEnumerable<DomainViewModel>> GetDomainsByTenantIdAsync(int tenantId);
        Task<TeamMemberViewModel[]> GetDomainTeamAsync(int domainId);
    }
}
