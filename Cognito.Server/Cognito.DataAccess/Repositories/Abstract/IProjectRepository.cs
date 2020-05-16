using Cognito.DataAccess.Entities;
using System.Linq;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public interface IProjectRepository : ICrudRepository<Project>
    {
        IQueryable<Project> GetProjectsByDomainId(int domainId);
        IQueryable<Project> GetUserProjects();
    }
}
