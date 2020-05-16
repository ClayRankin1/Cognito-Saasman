using Cognito.DataAccess.DbContext.Abstract;
using Cognito.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cognito.DataAccess.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<TResult> ConvertToLookup<TEnum, TResult>() where TResult : LookupBase<TEnum>, new()
        {
            return Enum
                .GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new TResult
                {
                    Id = e,
                    Label = e.ToString().InsertSpacesBeforeCapitals()
                });
        }
    }
}
