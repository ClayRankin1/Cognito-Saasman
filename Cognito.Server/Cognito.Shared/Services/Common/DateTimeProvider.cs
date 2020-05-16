using Cognito.Shared.Services.Common.Abstract;
using System;

namespace Cognito.Shared.Services.Common
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
