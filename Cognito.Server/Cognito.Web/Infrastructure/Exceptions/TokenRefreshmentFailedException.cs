using Cognito.Shared.Exceptions;
using System;

namespace Cognito.Web.Infrastructure.Exceptions
{
    [Serializable]
    public class TokenRefreshmentFailedException : CognitoException
    {
        public TokenRefreshmentFailedException(string message): base(message)
        {

        }
    }
}
