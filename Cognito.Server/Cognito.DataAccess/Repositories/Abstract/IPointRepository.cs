

using System.Collections.Generic;
using System.Threading.Tasks;
using Cognito.DataAccess.Entities;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public interface IPointRepository : ICrudRepository<Point>
    {
        Task<int> GetCountAsync(int projectId, int? parentId);
        Task<Point> GetPointDetailsByPointIdAsync(int id);
        Task<IEnumerable<Point>> GetPointDetailsByDetailIdsAsync(IEnumerable<int> ids);
        Task<IEnumerable<Point>> GetByProjectIdAsync(int projectId);
    }
}