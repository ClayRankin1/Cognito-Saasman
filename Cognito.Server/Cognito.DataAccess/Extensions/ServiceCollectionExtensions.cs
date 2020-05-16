using Cognito.DataAccess.DbContext;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Repositories;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.DataAccess.Services;
using Cognito.DataAccess.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cognito.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCognitoDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CognitoDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CognitoConnection"), options =>
                {
                    options.MigrationsHistoryTable("MigrationsHistory", DbSchemas.System);
                });
            });

            services.AddScoped<ICognitoDbContext, CognitoDbContext>(provider =>
            {
                var contextFactory = new CognitoDbContextFactory();
                return contextFactory.CreateDbContext(Array.Empty<string>());
            });

            using (var provider = services.BuildServiceProvider())
            using (var context = provider.GetService<CognitoDbContext>())
            {
                context.Database.Migrate();
            }

            return services;
        }

        public static IServiceCollection AddCognitoDataAccessRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IWebsiteRepository, WebsiteRepository>()
                .AddScoped<IDomainRepository, DomainRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<ITenantRepository, TenantRepository>()
                .AddScoped<IContactRepository, ContactRepository>()
                .AddScoped<ITaskRepository, TaskRepository>()
                .AddScoped<ISubtaskRepository, SubtaskRepository>()
                .AddScoped<IAccruedTimeRepository, AccruedTimeRepository>()
                .AddScoped<ILicenseRepository, LicenseRepository>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IDetailRepository, DetailRepository>()
                .AddScoped<IPointRepository, PointRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IMessageRepository, MessageRepository>();

            return services;
        }

        public static IServiceCollection AddCognitoDataAccessServices(this IServiceCollection services)
        {
            services
                // has to be singleton because it is used in middleware
                .AddSingleton<IDatabaseDeployer, DatabaseDeployer>()
                .AddScoped<ITransactionProvider, TransactionProvider>()
                .AddScoped<IDbConnectionFactory, DbConnectionFactory>()
                .AddScoped<IStoreProcedureRunner, StoreProcedureRunner>();

            return services;
        }

        public static IServiceCollection AddCognitoDataAccessInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCognitoDbContext(configuration)
                .AddCognitoDataAccessRepositories()
                .AddCognitoDataAccessServices();

            return services;
        }
    }
}
