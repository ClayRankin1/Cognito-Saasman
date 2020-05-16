using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cognito.DataAccess.Repositories
{
    public class DetailRepository : CrudRepository<Detail>, IDetailRepository
    {
        public DetailRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }

        protected override IQueryable<Detail> GetAllInternal() => _context.Details.Include(d => d.Task);
    }
}
