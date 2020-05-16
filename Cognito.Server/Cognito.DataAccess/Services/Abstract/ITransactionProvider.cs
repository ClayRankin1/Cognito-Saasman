using System.Data;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Services.Abstract
{
    public interface ITransactionProvider
    {
        IDbTransaction CurrentTransaction { get; }

        void BeginTransaction();

        Task CommitAsync();

        Task RollbackAsync();
    }
}
