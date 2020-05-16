using AutoMapper;
using Cognito.Business.Extensions;
using Cognito.DataAccess.Extensions;
using Cognito.Shared.Extensions;
using Cognito.Web.Extensions;
using Cognito.Web.Helpers;
using Cognito.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Net;

namespace Cognito.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHealthChecks();

            services
                .AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                    options.Filters.Add<ValidateModelFilterAttribute>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            var cognitoAssemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => assembly.FullName.StartsWith("Cognito"))
                .ToArray();

            services
                .AddSwagger()
                .AddAutoMapper(cognitoAssemblies)
                .AddCognitoCors()
                .AddCognitoOptions(Configuration)
                .AddCognitoDbContext(Configuration)
                .AddCognitoSharedServices()
                .AddCognitoDataAccessRepositories()
                .AddCognitoDataAccessServices()
                .AddCognitoBusinessDataServices()
                .AddCognitoBusinessServices()
                .AddCognitoWebServices()
                .AddCognitoAuthentication()
                .AddChangeScripts();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            // TODO: we don't want to show exception message in Production environment
            else
            {
                application.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            application
                .UseSwaggerAndUI()
                .UseRouting()
                .UseCors("CorsPolicy")
                .UseCustomExceptionMiddleware()
                .UseAuthentication()
                .UseHealthChecks("/health")
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                });
        }
    }
}