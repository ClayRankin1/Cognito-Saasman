using Amazon;
using Amazon.S3;
using Cognito.Business.DataServices;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Services;
using Cognito.Business.Services.Abstract;
using Cognito.Business.Services.Storage;
using Cognito.Business.Services.Storage.Abstract;
using Cognito.DataAccess.DbContext;
using Cognito.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Cognito.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCognitoBusinessDataServices(this IServiceCollection services)
        {
            services
                .AddScoped<IWebsiteDataService, WebsiteDataService>()
                .AddScoped<IDomainDataService, DomainDataService>()
                .AddScoped<IProjectDataService, ProjectDataService>()
                .AddScoped<ITenantDataService, TenantDataService>()
                .AddScoped<IContactDataService, ContactDataService>()
                .AddScoped<ITaskDataService, TaskDataService>()
                .AddScoped<IPermissionsService, PermissionsService>()
                .AddScoped<ILicenseDataService, LicenseDataService>()
                .AddScoped<IAccruedTimeDataService, AccruedTimeDataService>()
                .AddScoped<IDocumentDataService, DocumentDataService>()
                .AddScoped<IDetailDataService, DetailDataService>()
                .AddScoped<IPointDataService, PointDataService>()
                .AddScoped<IUserDataService, UserDataService>();

            return services;
        }

        public static IServiceCollection AddCognitoBusinessServices(this IServiceCollection services)
        {
            services
                .AddScoped<ILookupService, LookupService>()
                .AddScoped<IStorageService, AmazonStorageService>()
                .AddScoped<IDocumentService, DocumentService>()
                .AddScoped<IWebsiteService, WebsiteService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<IJsonService, JsonService>()
                .AddScoped<IAmazonS3>(provider => 
                {
                    var options = provider.GetRequiredService<IOptions<AwsOptions>>();
                    return new AmazonS3Client(options.Value.AccessKey, options.Value.SecretAccessKey, RegionEndpoint.USEast1);
                })
                .AddScoped<ITaskService, TaskService>();

            return services;
        }

        public static IServiceCollection AddCognitoNewAuthentication(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var passwordOptions = provider.GetRequiredService<IOptions<SecurityOptions>>().Value;
            var newBuilder = services
                .AddIdentityCore<DataAccess.Entities.User>(options =>
                {
                    options.Password.RequiredLength = passwordOptions.PasswordOptions.RequiredLength;
                    options.Password.RequiredUniqueChars = passwordOptions.PasswordOptions.RequiredUniqueChars;
                    options.Password.RequireDigit = passwordOptions.PasswordOptions.RequireDigit;
                    options.Password.RequireLowercase = passwordOptions.PasswordOptions.RequireLowercase;
                    options.Password.RequireNonAlphanumeric = passwordOptions.PasswordOptions.RequireNonAlphanumeric;
                    options.Password.RequireUppercase = passwordOptions.PasswordOptions.RequireUppercase;
                    options.User.RequireUniqueEmail = passwordOptions.RequireUniqueEmail;
                    options.SignIn.RequireConfirmedEmail = passwordOptions.RequireConfirmedEmail;
                })
                .AddDefaultTokenProviders();

            new IdentityBuilder(newBuilder.UserType, typeof(DataAccess.Entities.Role), newBuilder.Services)
                .AddEntityFrameworkStores<CognitoDbContext>()
                .AddRoleValidator<RoleValidator<DataAccess.Entities.Role>>()
                .AddRoleManager<RoleManager<DataAccess.Entities.Role>>()
                .AddSignInManager<SignInManager<DataAccess.Entities.User>>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(passwordOptions.SecurityKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("X-Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
