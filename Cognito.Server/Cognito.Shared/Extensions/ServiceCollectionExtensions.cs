using Cognito.Shared.Services.Common;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Cognito.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCognitoSharedServices(this IServiceCollection services)
        {
            services
                .AddScoped<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
