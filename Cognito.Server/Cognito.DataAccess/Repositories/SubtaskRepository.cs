using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;

namespace Cognito.DataAccess.Repositories
{
    public class SubtaskRepository : CrudRepository<Subtask>, ISubtaskRepository
    {
        public SubtaskRepository(
            ICognitoDbContext context,
            ICurrentUserService currentUserService) : base(context, currentUserService)
        {
        }
    }
}
