using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Filtering;
using Cognito.DataAccess.Repositories.Results;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IDataService
    {
    }

    public interface IDataService<in TEntity, TOutput> : IDataService
        where TEntity : class, IEntity, new()
        where TOutput : class, new()
    {
        Task<TOutput[]> GetAllAsync();

        Task<PaginatedList<TOutput>> FilterAsync(DataFilter filter);

        Task<TOutput> GetAsync(int id);

        Task<TOutput> CreateAsync(TEntity entity);

        Task<TOutput> UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);
    }
}
