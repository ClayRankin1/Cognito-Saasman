using Cognito.Web.Infrastructure.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<CustomExceptionMiddleware>();

        public static IApplicationBuilder UseSwaggerAndUI(this IApplicationBuilder builder)
        {
            builder
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cognito API v1");
                    // options.RoutePrefix = string.Empty;
                });

            return builder;
        }
    }
}
