using Cognito.DataSeeder.Options;
using Cognito.DataSeeder.Services;
using Cognito.DataSeeder.Services.Abstract;
using Cognito.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cognito.DataSeeder.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCognitoDataSeederServices(this IServiceCollection services)
        {
            services
                .AddScoped<ITestDataSeeder, TestDataSeeder>();

            return services;
        }

        public static IServiceCollection AddCognitoDataSeederOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<SecurityOptions>(configuration.GetSection(nameof(SecurityOptions)))
                .Configure<DefaultDataOptions>(configuration.GetSection(nameof(DefaultDataOptions)))
                .Configure<DataSeedingOptions>(configuration.GetSection(nameof(DataSeedingOptions)));

            return services;
        }
    }
}
