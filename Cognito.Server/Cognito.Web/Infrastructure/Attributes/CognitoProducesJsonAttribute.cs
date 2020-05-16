using Microsoft.AspNetCore.Mvc;

namespace Cognito.Web.Infrastructure.Attributes
{
    public sealed class CognitoProducesJsonAttribute : ProducesAttribute
    {
        public CognitoProducesJsonAttribute() : base("application/json")
        {

        }
    }
}
