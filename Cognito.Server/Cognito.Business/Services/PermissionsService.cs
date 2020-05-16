using Cognito.Business.Services.Abstract;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.Shared.Exceptions;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.Services
{
    // TODO: FIXME - Introduce caching in permissions service...
    public class PermissionsService : IPermissionsService
    {
        private readonly ICognitoDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public PermissionsService(
            ICognitoDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> HasAccessToTaskAsync(int taskId)
        {
            if (_currentUserService.IsInOneOfRoles(UserRoles.SysAdmin))
            {
                return true;
            }

            return await _context.ProjectUsers
                .TagWith($"Has UserId: {_currentUserService.UserId} access to TaskId: {taskId}")
                .AsNoTracking()
                .Where(up => up.UserId == _currentUserService.UserId)
                .SelectMany(up => up.Project.Tasks)
                .AnyAsync(t => t.Id == taskId);
        }

        public async Task<bool> HasAccessToProjectAsync(int projectId)
        {
            if (_currentUserService.IsInOneOfRoles(UserRoles.SysAdmin))
            {
                return true;
            }

            // TODO: FIXME - Check User roles???
            // The user might be allowed just by it's user role such as Admin....

            return await _context.Projects
                .AsNoTracking()
                .Where(p => p.Id == projectId)
                .SelectMany(p => p.Users)
                .Where(up => up.UserId == _currentUserService.UserId)
                .AnyAsync();
        }

        public async Task EnsureTaskAccessAsync(int taskId)
        {
            var hasAccess = await HasAccessToTaskAsync(taskId);
            if (!hasAccess)
            {
                throw new ForbiddenException();
            }
        }

        public async Task<bool> HasAccessToDomainAsync(int domainId)
        {
            if (_currentUserService.IsInOneOfRoles(UserRoles.SysAdmin))
            {
                return true;
            }

            // TODO: FIXME - Check for Admin role...

            var userDomain = await _context.UserDomains
                .AsNoTracking()
                .Where(ud => ud.UserId == _currentUserService.UserId && ud.DomainId == domainId)
                .FirstOrDefaultAsync();

            if (userDomain == null)
            {
                return false;
            }

            // TODO: FIXME - Check user role...

            return true;
        }

        public async Task<bool> IsAdminForTenant(Tenant tenant)
        {
            return await _context.Tenants
                .AsNoTracking()
                .Where(t => t.Id == tenant.Id)
                .AnyAsync(t => t.Domains
                    .Any(d => d.UserDomains
                        .Any(ud => ud.UserId == _currentUserService.UserId && ud.RoleId == (int)UserRoles.TenantAdmin)));
        }

        public async Task<bool> IsAdminForDomain(int domainId)
        {
            return await _context.UserDomains.AnyAsync(ud => ud.UserId == _currentUserService.UserId && ud.RoleId == (int)UserRoles.DomainAdmin);
        }

        public bool IsInRole(UserRoles role) => _currentUserService.IsInRole(role);

        public async Task EnsureProjectsAccessAsync(IEnumerable<int> requestedProjectIds)
        {
            // TODO: FIXME - Some "SUPER" role could have access to all the projects
            //if ("I am SUPER")
            //{
            //    return true;
            //}

            var uniqueRequestedProjectIds = requestedProjectIds.Distinct();
            var projectIds = await _context.ProjectUsers
                .Where(up => up.UserId == _currentUserService.UserId && uniqueRequestedProjectIds.Contains(up.ProjectId))
                .Select(up => up.ProjectId)
                .ToArrayAsync();

            var hasAccess = projectIds.Count() == uniqueRequestedProjectIds.Count();
            if (!hasAccess)
            {
                throw new ForbiddenException();
            }
        }

        public async Task EnsureDetailsAccessAsync(IEnumerable<int> requestedDetailIds)
        {
            // TODO: FIXME - Some "SUPER" role could have access to all the projects
            //if ("I am SUPER")
            //{
            //    return true;
            //}

            var uniqueRequestedDetailIds = requestedDetailIds.Distinct();
            var projectIds = await _context.Details
                .Where(d => uniqueRequestedDetailIds.Contains(d.Id))
                .SelectMany(d => d.Task.Project.Users)
                .Where(u => u.UserId == _currentUserService.UserId)
                .Select(u => u.ProjectId)
                .Distinct()
                .ToArrayAsync();

            var hasAccess = projectIds.Count() == uniqueRequestedDetailIds.Count();
            if (!hasAccess)
            {
                throw new ForbiddenException();
            }
        }
    }
}
