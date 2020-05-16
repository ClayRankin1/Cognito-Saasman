using Dapper;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Services.Abstract
{
    public interface IStoreProcedureRunner
    {
        int Execute(string procName, object param = null);

        Task<int> ExecuteAsync(string procName, object param = null);

        Task<TResult> QueryFirstAsync<TResult>(string procName, object param = null);

        Task<SqlMapper.GridReader> QueryMultipleAsync(string procName, object param = null);
    }
}
