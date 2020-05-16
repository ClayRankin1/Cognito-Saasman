using Cognito.DataAccess.Repositories.Results;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Repositories.Abstract
{
    public interface ICrudRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(int id);

        Task<PaginatedList<TEntity>> GetPageAsync(int pageNumber, int pageSize, string sortField, string sortOrder);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void DeleteById(int id);

        TEntity Clone(TEntity entity);

        Task<bool> SaveAllAsync();
    }
}
