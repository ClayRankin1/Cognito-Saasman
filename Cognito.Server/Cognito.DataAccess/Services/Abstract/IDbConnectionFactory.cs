using System.Data;

namespace Cognito.DataAccess.Services.Abstract
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
