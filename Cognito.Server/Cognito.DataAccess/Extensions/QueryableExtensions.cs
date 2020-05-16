using Cognito.DataAccess.Filtering;
using Cognito.Shared.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Cognito.DataAccess.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, string direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));
            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == "ASC" ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }

        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, DataFilter filter)
        {
            return query
                .ApplyFilters(filter.Filter)
                .ApplySorting(filter.Sorts)
                .ApplyPaging(filter.Paging);
        }

        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> queryable, PropertyFilter filter)
        {
            if (filter?.Logic != null)
            {
                var filters = GetAllFilters(filter);
                var values = filters.Select(f => f.Value).ToArray();
                var where = Transform(filter, filters);
                queryable = queryable.Where(where, values);
            }

            return queryable;
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> queryable, IEnumerable<Sorting> sort)
        {
            if (sort?.Any() ?? false)
            {
                var ordering = string.Join(",", sort.Select(s => 
                {
                    var direction = s.Direction == SortDirection.Descending ? "DESC" : "ASC";
                    return $"{s.Field} {direction}";
                }));
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        private static IQueryable<T> ApplyPaging<T>(this IQueryable<T> queryable, Paging paging)
        {
            var skip = (paging.PageNumber - 1) * paging.PageSize;
            return queryable.Skip(skip).Take(paging.PageSize);
        }

        private static readonly IDictionary<PropertyFilterOperator, string> Operators = new Dictionary<PropertyFilterOperator, string>
        {
            { PropertyFilterOperator.Equal, "="},
            { PropertyFilterOperator.NotEqual, "!="},
            { PropertyFilterOperator.LessThan, "<"},
            { PropertyFilterOperator.LessThanOrEqual, "<="},
            { PropertyFilterOperator.GreaterThan, ">"},
            { PropertyFilterOperator.GreaterThanOrEqual, ">="},
            { PropertyFilterOperator.StartsWith, "StartsWith"},
            { PropertyFilterOperator.EndsWith, "EndsWith"},
            { PropertyFilterOperator.Contains, "Contains"},
            { PropertyFilterOperator.NotContains, "Contains"}
        };

        public static IList<PropertyFilter> GetAllFilters(PropertyFilter filter)
        {
            var filters = new List<PropertyFilter>();
            GetFilters(filter, filters);
            return filters;
        }

        private static void GetFilters(PropertyFilter filter, IList<PropertyFilter> filters)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var item in filter.Filters)
                {
                    GetFilters(item, filters);
                }
            }
            else
            {
                filters.Add(filter);
            }
        }

        public static string Transform(PropertyFilter filter, IList<PropertyFilter> filters)
        {
            if (filter.Filters?.Any() ?? false)
            {
                return "(" + string.Join(" " + GetLogic(filter.Logic.Value) + " ",
                    filter.Filters.Select(f => Transform(f, filters)).ToArray()) + ")";
            }

            int index = filters.IndexOf(filter);
            var comparison = Operators[filter.Operator];
            
            if (filter.Operator == PropertyFilterOperator.NotContains)
            {
                return string.Format("({0} != null && !{0}.{1}(@{2}))", filter.Field, comparison, index);
            }
            
            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                return string.Format("({0} != null && {0}.{1}(@{2}))",
                filter.Field, comparison, index);
            }
            
            return string.Format("{0} {1} @{2}", filter.Field, comparison, index);
        }

        private static string GetLogic(LogicOperation operation)
        {
            switch (operation)
            {
                case LogicOperation.And:
                    return "&&";
                case LogicOperation.Or:
                    return "||";
                default:
                    throw new InvalidOperationException($"Invalid operation: {operation}");
            }
        }
    }
}
