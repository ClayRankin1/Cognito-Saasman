using Cognito.DataAccess.Entities;
using System.Linq;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public interface IDomainRepository : ICrudRepository<Domain>
    {
        IQueryable<Domain> GetAdminDomains();
        IQueryable<Domain> GetDomainsByTenantId(int tenantId);
    }
}
