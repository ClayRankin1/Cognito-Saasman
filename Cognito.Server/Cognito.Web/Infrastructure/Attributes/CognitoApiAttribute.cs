using Microsoft.AspNetCore.Mvc;

namespace Cognito.Web.Infrastructure.Attributes
{
    public sealed class CognitoApiAttribute : RouteAttribute
    {
        public CognitoApiAttribute() : base("api/[controller]")
        {

        }
    }
}
