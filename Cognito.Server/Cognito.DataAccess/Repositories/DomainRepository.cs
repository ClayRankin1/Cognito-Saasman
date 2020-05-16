using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Security;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cognito.DataAccess.Repositories
{
    public class DomainRepository : CrudRepository<Domain>, IDomainRepository
    {
        public DomainRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }

        protected override IQueryable<Domain> GetAllInternal()
        {
            if (_currentUserService.IsInRole(UserRoles.TenantAdmin))
            {
                // TODO: FIXME - How we know which domains to load for administrator account?
            }

            return _context.UserDomains
                .AsNoTracking()
                .Where(ud => ud.UserId == _currentUserService.UserId)
                .Select(ud => ud.Domain);
        }

        public override void Update(Domain entity)
        {
            base.Update(entity);

            var entry = _context.Entry(entity);
            entry.Property(e => e.TenantId).IsModified = false;
            entry.Property(e => e.AdminUserId).IsModified = false;
        }

        public IQueryable<Domain> GetDomainsByTenantId(int tenantId)
        {
            return _context.Domains
                .Include(d => d.DomainLicenses).ThenInclude(dl => dl.License)
                .Include(d => d.UserDomains).ThenInclude(ud => ud.User)
                .Where(d => d.TenantId == tenantId);
        }

        public IQueryable<Domain> GetAdminDomains()
        {
            return _context.Domains.Include(d => d.Tenant);
        }
    }
}
