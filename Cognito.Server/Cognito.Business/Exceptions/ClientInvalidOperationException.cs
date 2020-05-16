using Cognito.Shared.Exceptions;
using System;

namespace Cognito.Business.Exceptions
{
    [Serializable]
    public class ClientInvalidOperationException : CognitoException
    {
        public string Title { get; }

        public ClientInvalidOperationException(string message) : this(message, null)
        {
        }

        public ClientInvalidOperationException(string message, string title) : base(message)
        {
            Title = title;
        }
    }
}
