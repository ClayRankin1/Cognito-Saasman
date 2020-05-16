using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IPointDataService : IDataService<Point, PointViewModel>
    {
        Task<int> GetCountAsync(int projectId, int? parentId);
        Task<PointViewModel> GetPointDetailsByPointIdAsync(int id);
        Task<IEnumerable<PointViewModel>> GetPointDetailsByDetailIdsAsync(IEnumerable<int> ids);
        Task<PointViewModel> ReorderAsync(Point point);
        Task<PointViewModel> AddPointDetailsAsync(int id, IEnumerable<int> detailIds);
        Task<IEnumerable<PointViewModel>> GetByProjectIdAsync(int projectId);
    }
}