using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cognito.DataAccess.Services
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly ICognitoDbContext _context;

        public DbConnectionFactory(ICognitoDbContext context)
        {
            _context = context;
        }

        public IDbConnection GetConnection() => _context.Database.GetDbConnection();
    }
}
