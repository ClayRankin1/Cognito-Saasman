using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class TaskStatusEntityConfiguration : IEntityTypeConfiguration<TaskStatus>
    {
        public void Configure(EntityTypeBuilder<TaskStatus> builder)
        {
            builder
                .Property(e => e.Id)
                .HasConversion<int>();

            builder.HasData(EnumHelper.ConvertToLookup<TaskStatusId, TaskStatus>());
        }
    }
}
