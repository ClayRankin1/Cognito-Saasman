using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class TaskWebsiteEntityConfiguration : IEntityTypeConfiguration<TaskWebsite>
    {
        public void Configure(EntityTypeBuilder<TaskWebsite> builder)
        {
            builder.HasKey(tw => new { tw.TaskId, tw.WebsiteId });
        }
    }
}
