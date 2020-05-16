using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.DataAccess.Services.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cognito.DataAccess.Repositories
{
    public class ProjectRepository : CrudRepository<Project>, IProjectRepository
    {
        private readonly IStoreProcedureRunner _storeProcedureRunner;

        public ProjectRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService,
            IStoreProcedureRunner storeProcedureRunner) : base(context, currentUserService)
        {
            _storeProcedureRunner = storeProcedureRunner;
        }

        public IQueryable<Project> GetUserProjects() => _context.ProjectUsers
            .AsNoTracking()
            .Where(up => up.UserId == _currentUserService.UserId)
            .Select(up => up.Project);

        public IQueryable<Project> GetProjectsByDomainId(int domainId) => _context.Projects
            .AsNoTracking()
            .Include(p => p.Owner)
            .Include(p => p.Proxy)
            .Include(p => p.Users)
            .Where(p => p.DomainId == domainId);

        public override void Update(Project entity)
        {
            _storeProcedureRunner.Execute(StoredProcedures.MergeProjectUsers, new
            {
                ProjectId = entity.Id,
                UserIds = string.Join(",", entity.Users.Select(t => t.UserId))
            });

            entity.Users.Clear();

            base.Update(entity);
        }
    }
}
