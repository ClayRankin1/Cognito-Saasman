using Microsoft.AspNetCore.Hosting;
using System;

namespace Cognito.Web.Extensions
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsDevelopment(this IWebHostEnvironment environment)
        {
            return environment.EnvironmentName.Equals("development", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
