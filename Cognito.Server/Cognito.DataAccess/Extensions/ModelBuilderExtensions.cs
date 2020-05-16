using Cognito.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cognito.DataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder DisableCascadeDeletes(this ModelBuilder builder)
        {
            builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(key => !key.IsOwnership && key.DeleteBehavior == DeleteBehavior.Cascade)
                .ForEach(key =>
                {
                    key.DeleteBehavior = DeleteBehavior.Restrict;
                });

            return builder;
        }
    }
}
