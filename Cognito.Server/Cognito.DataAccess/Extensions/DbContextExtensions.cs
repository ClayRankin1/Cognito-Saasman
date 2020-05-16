using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.Linq;
using System.Reflection;

namespace Cognito.DataAccess.Extensions
{
    public static class DbContextExtensions
    {
        public static IQueryable Set(this ICognitoDbContext context, Type T)
        {
            // Get the generic type definition
            var method = typeof(ICognitoDbContext).GetMethod(nameof(ICognitoDbContext.Set), BindingFlags.Public | BindingFlags.Instance);

            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(T);

            return method.Invoke(context, null) as IQueryable;
        }
    }
}
