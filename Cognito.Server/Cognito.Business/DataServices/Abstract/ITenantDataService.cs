using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;

namespace Cognito.Business.DataServices.Abstract
{
    public interface ITenantDataService : IDataService<Tenant, TenantViewModel>
    {

    }
}
