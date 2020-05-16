using Cognito.DataAccess.Services.Abstract;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Services
{
    public class StoreProcedureRunner : IStoreProcedureRunner, IDisposable
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IDbConnection _connection;
        private readonly ITransactionProvider _transactionProvider;

        public StoreProcedureRunner(
            IDbConnectionFactory dbConnectionFactory,
            ITransactionProvider transactionProvider)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _connection = _dbConnectionFactory.GetConnection();
            _transactionProvider = transactionProvider;
        }

        public Task<TResult> QueryFirstAsync<TResult>(string procName, object param = null)
        {
            return _connection.QueryFirstAsync<TResult>(procName, param, _transactionProvider.CurrentTransaction, commandType: CommandType.StoredProcedure);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(string procName, object param = null)
        {
            return _connection.QueryMultipleAsync(procName, param, _transactionProvider.CurrentTransaction, commandType: CommandType.StoredProcedure);
        }

        public Task<int> ExecuteAsync(string procName, object param = null)
        {
            return _connection.ExecuteAsync(procName, param, _transactionProvider.CurrentTransaction, commandType: CommandType.StoredProcedure);
        }

        public int Execute(string procName, object param = null)
        {
            return _connection.Execute(procName, param, _transactionProvider.CurrentTransaction, commandType: CommandType.StoredProcedure);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _connection?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StoreProcedureRunner()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
