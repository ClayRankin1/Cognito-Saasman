using Cognito.Business.Models;
using Cognito.DataAccess.DbContext.Abstract;
using System.Linq;

namespace Cognito.Business.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyLocalFilter<T>(this IQueryable<T> query, LocalFilter filter) where T : ITaskable
        {
            if (filter.DomainId.HasValue)
            {
                query = query.Where(td => td.Task.Project.DomainId == filter.DomainId.Value);
            }

            if (filter.ProjectId.HasValue)
            {
                query = query.Where(td => td.Task.ProjectId == filter.ProjectId.Value);
            }

            if (filter.TaskId.HasValue)
            {
                query = query.Where(td => td.TaskId == filter.TaskId.Value);
            }

            return query;
        }
    }
}
