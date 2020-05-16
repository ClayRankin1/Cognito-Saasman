using Cognito.Business.Services.Abstract;
using Cognito.DataAccess.DbContext.Abstract;
using Cognito.DataAccess.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cognito.Business.Services
{
    public class LookupService : ILookupService
    {
        private readonly ICognitoDbContext _context;

        public LookupService(ICognitoDbContext context)
        {
            _context = context;
        }

        public Dictionary<string, object> GetAllLookups()
        {
            var lookups = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => (x.BaseType?.IsGenericType ?? false) && x.BaseType.GetGenericTypeDefinition() == typeof(LookupBase<>))
                .ToArray();

            var result = new Dictionary<string, object>();

            foreach (var lookupType in lookups)
            {
                var data = _context.Set(lookupType);
                result.Add(lookupType.Name, data);
            }

            return result;
        }
    }
}
