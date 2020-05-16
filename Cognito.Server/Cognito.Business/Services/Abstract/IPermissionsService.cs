using Cognito.DataAccess.Entities;
using Cognito.Shared.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Abstract
{
    public interface IPermissionsService
    {
        Task<bool> HasAccessToTaskAsync(int taskId);

        Task<bool> HasAccessToProjectAsync(int projectId);

        Task<bool> HasAccessToDomainAsync(int domainId);

        Task EnsureTaskAccessAsync(int taskId);

        Task EnsureProjectsAccessAsync(IEnumerable<int> requestedProjectIds);

        Task EnsureDetailsAccessAsync(IEnumerable<int> requestedDetailIds);

        Task<bool> IsAdminForTenant(Tenant tenant);
        
        bool IsInRole(UserRoles role);
        Task<bool> IsAdminForDomain(int domainId);
    }
}
