using Cognito.Business.ViewModels;
using Cognito.Business.ViewModels.Task;
using Cognito.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IProjectDataService : IDataService<Project, ProjectViewModel>
    {
        Task<IEnumerable<ProjectViewModel>> GetProjectsByDomainIdAsync(int domainId);
        Task<TeamMemberViewModel[]> GetProjectTeamAsync(int projectId);
    }
}
