using System;

namespace Cognito.Shared.Exceptions
{
    /// <summary>
    /// Base class for application exceptions.
    /// </summary>
    [Serializable]
    public class CognitoException : Exception
    {
        protected CognitoException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CognitoException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        protected CognitoException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CognitoException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="inner">Inner exception.</param>
        protected CognitoException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CognitoException"/> class.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Serialization context.</param>
        protected CognitoException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
