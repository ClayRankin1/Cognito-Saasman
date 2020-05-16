using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.Collections.Generic;

namespace Cognito.DataSeeder.Extensions
{
    public static class DateAuditableExtensions
    {
        public static IEnumerable<TEntity> FillDateAuditableFields<TEntity>(this IEnumerable<TEntity> source, DateTime now) where TEntity: IDateAuditable
        {
            foreach (var item in source)
            {
                item.DateAdded = now;
                item.DateUpdated = now;

                yield return item;
            }
        }
    }
}
