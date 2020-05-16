using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net;

namespace Cognito.Web.Infrastructure.Swagger
{
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            var unauthorizedKey = ((int)HttpStatusCode.Unauthorized).ToString();

            if (authAttributes.Any() && operation.Responses.Any(r => r.Key != unauthorizedKey))
            {
                operation.Responses.TryAdd(unauthorizedKey, new OpenApiResponse { Description = nameof(HttpStatusCode.Unauthorized) });
            }
        }
    }
}
