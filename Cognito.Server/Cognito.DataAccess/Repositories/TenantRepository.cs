using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cognito.DataAccess.Repositories
{
    public class TenantRepository : CrudRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService) : base(context, currentUserService) { }

        protected override IQueryable<Tenant> GetAllInternal()
        {
            return base.GetAllInternal()
                .Include(t => t.Address).ThenInclude(a => a.State)
                .Include(t=> t.Domains).ThenInclude(a => a.UserDomains).ThenInclude(t => t.User);
        }
    }
}
