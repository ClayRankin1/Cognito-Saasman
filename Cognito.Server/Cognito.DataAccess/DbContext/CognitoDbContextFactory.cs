using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Cognito.DataAccess.DbContext
{
    public class CognitoDbContextFactory : IDesignTimeDbContextFactory<CognitoDbContext>
    {
        public CognitoDbContext CreateDbContext(string[] args)
        {
            var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            if (!string.IsNullOrEmpty(environment))
            {
                configurationBuilder.AddJsonFile($"appsettings.{environment}.json");
            }

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("CognitoConnection");
            var optionsBuilder = new DbContextOptionsBuilder<CognitoDbContext>();
            
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(CognitoLoggerFactory)
                .UseSqlServer(connectionString);

            return new CognitoDbContext(optionsBuilder.Options);
        }

        public static readonly ILoggerFactory CognitoLoggerFactory = LoggerFactory.Create(builder => 
        {
            builder.AddConsole();
        });
    }
}
