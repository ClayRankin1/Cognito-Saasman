using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class TaskDocumentEntityConfiguration : IEntityTypeConfiguration<TaskDocument>
    {
        public void Configure(EntityTypeBuilder<TaskDocument> builder)
        {
            builder.HasKey(td => new { td.TaskId, td.DocumentId });
        }
    }
}
