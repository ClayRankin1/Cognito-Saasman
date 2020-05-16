using Amazon;
using Amazon.SimpleEmail;
using Cognito.Business.Extensions;
using Cognito.DataAccess.Services.Abstract;
using Cognito.Shared.Options;
using Cognito.Shared.Services.Common.Abstract;
using Cognito.Web.Infrastructure.Filters;
using Cognito.Web.Infrastructure.Swagger;
using Cognito.Web.Services;
using Cognito.Web.Services.Abstract;
using Cognito.Web.Services.Security;
using Cognito.Web.Services.Security.Abstract;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SendGrid;
using System;
using System.IO;
using System.Reflection;

namespace Cognito.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCognitoWebServices(this IServiceCollection services)
        {
            services
                .AddScoped<ISendGridClient>(provider =>
                {
                    var options = provider.GetRequiredService<IOptions<SendGridOptions>>();
                    return new SendGridClient(options.Value.MailSendApiKey);
                })
                .AddScoped<IAmazonSimpleEmailService>(provider =>
                {
                    var options = provider.GetRequiredService<IOptions<AwsOptions>>();
                    return new AmazonSimpleEmailServiceClient(options.Value.AccessKey, options.Value.SecretAccessKey, RegionEndpoint.USEast1);
                })
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<IPasswordService, PasswordService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }

        public static IServiceCollection AddCognitoOptions(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: Rename settings to options and use nameof() operator
            services
                .Configure<AppOptions>(configuration.GetSection("AppSettings"))
                .Configure<AwsOptions>(configuration.GetSection("AWSSettings"))
                .Configure<SendGridOptions>(configuration.GetSection("SendGridSettings"))
                .Configure<SecurityOptions>(configuration.GetSection("SecuritySettings"))
                .Configure<ConnectionStringOptions>(configuration.GetSection("ConnectionStrings"))
                .Configure<FormOptions>(options =>
                {
                    options.ValueLengthLimit = int.MaxValue;
                    options.MultipartBodyLengthLimit = int.MaxValue;
                    options.MultipartHeadersLengthLimit = int.MaxValue;
                });

            return services;
        }

        public static IServiceCollection AddCognitoAuthentication(this IServiceCollection services)
        {
            services
                .AddCognitoNewAuthentication();

            return services;
        }

        public static IServiceCollection AddCognitoCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection AddCustomizedApi(this IServiceCollection services)
        {
            services
                .AddMvc(config =>
                {
                    config.Filters.Add<ValidateModelFilterAttribute>();
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Cognito API",
                    Description = "This is description of Cognito REST API.",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Shayne Boyer",
                    //    Email = string.Empty,
                    //    Url = new Uri("https://twitter.com/spboyer"),
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Use under LICX",
                    //    Url = new Uri("https://example.com/license"),
                    //}
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.OperationFilter<AuthResponsesOperationFilter>();
                options.OperationFilter<CrudResponsesOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            })
            .AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        public static IServiceCollection AddChangeScripts(this IServiceCollection services)
        {
            var deployer = services
                .BuildServiceProvider()
                .GetRequiredService<IDatabaseDeployer>();

            deployer.DeployChangeScriptsAsync()
                .GetAwaiter()
                .GetResult();

            return services;
        }
    }
}
