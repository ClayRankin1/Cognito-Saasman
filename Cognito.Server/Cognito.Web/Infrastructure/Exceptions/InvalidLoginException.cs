using Cognito.Shared.Exceptions;
using System;

namespace Cognito.Web.Infrastructure.Exceptions
{
    [Serializable]
    public class InvalidLoginException : CognitoException
    {
        public InvalidLoginException()
        {

        }

        public InvalidLoginException(string message): base(message)
        {

        }
    }
}
