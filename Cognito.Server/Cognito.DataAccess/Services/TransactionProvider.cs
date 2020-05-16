using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Services.Abstract;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Services
{
    public class TransactionProvider : ITransactionProvider
    {
        private readonly ICognitoDbContext _context;
        private IDbContextTransaction _dbContextTransaction;

        public TransactionProvider(ICognitoDbContext context)
        {
            _context = context;
        }

        public IDbTransaction CurrentTransaction { get; private set; }

        public void BeginTransaction()
        {
            _dbContextTransaction = _context.Database.BeginTransaction();
            CurrentTransaction = _dbContextTransaction.GetDbTransaction();
        }

        public Task CommitAsync()
        {
            return _dbContextTransaction == null ? Task.CompletedTask : _dbContextTransaction.CommitAsync();
        }

        public Task RollbackAsync()
        {
            return _dbContextTransaction == null ? Task.CompletedTask : _dbContextTransaction.RollbackAsync();
        }
    }
}
