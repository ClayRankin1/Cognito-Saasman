using System;

namespace Cognito.Shared.Services.Common.Abstract
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
