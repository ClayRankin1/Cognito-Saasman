using Cognito.Business.Extensions;
using Cognito.DataAccess.Extensions;
using Cognito.DataSeeder.Extensions;
using Cognito.DataSeeder.Services.Abstract;
using Cognito.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cognito.DataSeeder
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static async Task Main(string[] args)
        {
            // Initialize Serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            // Create service collection
            Log.Information("Creating service collection");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            Log.Information("Building service provider");
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Print connection string to demonstrate configuration object is populated
            Console.WriteLine(configuration.GetConnectionString("CognitoConnection"));

            try
            {
                Log.Information("Starting DataSeeder");
                await serviceProvider.GetService<ITestDataSeeder>().SeedDataAsync();
                Log.Information("Ending DataSeeder");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running DataSeeder");
                throw ex;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder
                    .AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", true)
                .Build();

            // Configure IoC
            serviceCollection
                .AddSingleton(configuration)
                .AddCognitoDataSeederOptions(configuration)
                .AddCognitoSharedServices()
                .AddCognitoDataAccessInfrastructure(configuration)
                .AddCognitoBusinessDataServices()
                .AddCognitoNewAuthentication()
                .AddCognitoDataSeederServices();

        }
    }
}
