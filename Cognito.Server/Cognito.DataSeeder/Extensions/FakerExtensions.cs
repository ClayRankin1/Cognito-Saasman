using Bogus;
using Cognito.DataAccess.DbContext.Abstract;

namespace Cognito.DataSeeder.Extensions
{
    public static class FakerExtensions
    {
        public static Faker<T> RuleForAuditableFields<T>(this Faker<T> faker, int userId) where T : class, IAuditable
        {
            return faker
                .RuleFor(t => t.CreatedByUserId, f => userId)
                .RuleFor(t => t.UpdatedByUserId, f => userId)
                .RuleFor(t => t.DateAdded, f => f.Date.Past())
                .RuleFor(t => t.DateUpdated, f => f.Date.Recent());
        }
    }
}
