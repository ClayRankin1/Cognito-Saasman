using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.Business.ViewModels.Task;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Exceptions;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class ProjectDataService : DataServiceBase<Project, ProjectViewModel, IProjectRepository>, IProjectDataService
    {
        private readonly IPermissionsService _permissionsService;

        public ProjectDataService(
            IMapper mapper,
            IProjectRepository repository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IPermissionsService permissionsService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
            _permissionsService = permissionsService;
        }

        public override async Task<ProjectViewModel[]> GetAllAsync()
        {
            return await _repository.GetUserProjects()
               .ProjectTo<ProjectViewModel>(_mapper.ConfigurationProvider)
               .ToArrayAsync();
        }

        public Task<TeamMemberViewModel[]> GetProjectTeamAsync(int projectId)
        {
            return _repository
                .GetUserProjects()
                .Where(p => p.Id == projectId)
                .SelectMany(p => p.Users)
                .Select(up => up.User)
                .ProjectTo<TeamMemberViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjectsByDomainIdAsync(int domainId)
        {
            var projects = await _repository.GetProjectsByDomainId(domainId)
                .Where(p => p.OwnerId == _currentUserService.UserId ||
                            p.ProxyId == _currentUserService.UserId || 
                            p.Domain.UserDomains.Any(ud => ud.UserId == _currentUserService.UserId && ud.RoleId == (int)UserRoles.DomainAdmin))
                .Select(p => new Project
                {
                    Id = p.Id,
                    ArchivedOn = p.ArchivedOn,
                    ProjectNo = p.ProjectNo,
                    ClientNo = p.ClientNo,
                    Description = p.Description,
                    DomainId = p.DomainId,
                    FullName = p.FullName,
                    IsBillable = p.IsBillable,
                    Nickname = p.Nickname,
                    Owner = new User { Id = p.Owner.Id, FirstName = p.Owner.FirstName, LastName = p.Owner.LastName },
                    OwnerId = p.Owner.Id,
                    Proxy = new User { Id = p.Proxy.Id, FirstName = p.Proxy.FirstName, LastName = p.Proxy.LastName },
                    ProxyId = p.Proxy.Id,
                    Users = p.Users.Select(pu => new ProjectUser
                            {
                                UserId = pu.UserId,
                                PendingTasks = pu.User.UserTasks.Count(ut => ut.ProjectId == p.Id && ut.TaskStatusId == TaskStatusId.Pending)
                            }).ToList()
                }).ToListAsync();

            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }

        public override async Task<ProjectViewModel> CreateAsync(Project entity)
        {
            if (!await _permissionsService.IsAdminForDomain(entity.DomainId))
            {
                throw new ForbiddenException();
            }

            return await base.CreateAsync(entity);
        }

        public override async Task<ProjectViewModel> UpdateAsync(Project entity)
        {
            var project = await _repository.GetByIdAsync(entity.Id);

            if (!await _permissionsService.IsAdminForDomain(project.DomainId) && 
                project.OwnerId != _currentUserService.UserId &&
                project.ProxyId != _currentUserService.UserId)
            {
                throw new ForbiddenException();
            }

            if (IsRemovingUsersWithPendingTasks(entity))
            {
                throw new ForbiddenException();
            }

            return await base.UpdateAsync(entity);
        }

        public override async Task DeleteAsync(int id)
        {
            var project = await _repository.GetByIdAsync(id);

            if (!await _permissionsService.IsAdminForDomain(project.DomainId))
            {
                throw new ForbiddenException();
            }

            await base.DeleteAsync(id);
        }

        private bool IsRemovingUsersWithPendingTasks(Project entity)
        {
            var oldProjectUsers = _repository.GetAll()
                .Where(p => p.Id == entity.Id)
                .SelectMany(p => p.Users
                    .Where(pu => !pu.IsDeleted)
                    .Select(u => new 
                    { 
                        u.UserId, 
                        PendingTasks = u.User.UserTasks.Count(ut => ut.ProjectId == p.Id && ut.TaskStatusId == TaskStatusId.Pending)
                    }));

            var newUsers = entity.Users.Select(item => item.UserId).ToHashSet();
            foreach (var oldUser in oldProjectUsers)
            {
                if (!newUsers.Contains(oldUser.UserId) && oldUser.PendingTasks > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
