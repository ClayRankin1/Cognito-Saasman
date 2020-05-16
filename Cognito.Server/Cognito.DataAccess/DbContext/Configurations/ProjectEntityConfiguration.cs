using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasMany(p => p.Users)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
