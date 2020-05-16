using Cognito.DataAccess.Services.Abstract;
using Cognito.Shared.Options;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.DataAccess.Services
{
    public class DatabaseDeployer : IDatabaseDeployer
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<DatabaseDeployer> _logger;
        private readonly IOptions<ConnectionStringOptions> _options;
        private readonly string[] _splitter = { $"{Environment.NewLine}GO{Environment.NewLine}" };

        public DatabaseDeployer(
            IWebHostEnvironment environment,
            ILogger<DatabaseDeployer> logger,
            IOptions<ConnectionStringOptions> options)
        {
            _environment = environment;
            _logger = logger;
            _options = options;
        }

        public async Task DeployChangeScriptsAsync()
        {
            var sqlScriptFileNames = new DirectoryInfo(_environment.ContentRootPath)
                .GetFiles("*.sql", SearchOption.AllDirectories)
                .OrderBy(sqlFile => sqlFile.Name)
                .Select(sqlFile => sqlFile.FullName)
                .Distinct();

            using var connection = new SqlConnection(_options.Value.CognitoConnection);
            await connection.OpenAsync();

            foreach (var sqlScriptFileName in sqlScriptFileNames)
            {
                try
                {
                    _logger.LogInformation($"Deploying SQL Script: {sqlScriptFileName}");

                    var sqlScript = await File.ReadAllTextAsync(sqlScriptFileName);
                    var commandTexts = sqlScript.Split(_splitter, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var commandText in commandTexts)
                    {
                        await connection.ExecuteAsync(commandText);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to deploy file: ${sqlScriptFileName} with Error: ${ex.Message}");
                    throw;
                }
            }
        }
    }
}
