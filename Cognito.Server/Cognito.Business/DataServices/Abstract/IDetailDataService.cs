using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IDetailDataService : IDataService<Detail, DetailViewModel>
    {
        Task<DetailViewModel[]> GetDetailsByTaskIdAsync(int taskId, int? detailTypeId);

        Task<PointDetailViewModel[]> GetLinkedDetailsAsync(int projectId);

        Task SplitAsync(int[] detailIds);

        Task MergeAsync(int[] detailIds);

        Task<int> BulkDeleteDetailsAsync(IEnumerable<int> ids);

        Task<DetailViewModel> CopyDetailAsync(int detailId, int targetTaskId);
    }
}
