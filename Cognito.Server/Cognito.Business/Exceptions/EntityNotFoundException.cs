using Cognito.Shared.Exceptions;
using System;

namespace Cognito.Business.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : CognitoException
    {
        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}
