using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class TaskContactEntityConfiguration : IEntityTypeConfiguration<TaskContact>
    {
        public void Configure(EntityTypeBuilder<TaskContact> builder)
        {
            builder.HasKey(tc => new { tc.TaskId, tc.ContactId });
        }
    }
}
