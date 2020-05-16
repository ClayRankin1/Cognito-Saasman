using Cognito.DataAccess.Entities;
using System.Linq;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public interface IUserRepository : ICrudRepository<User>
    {
    }
}
