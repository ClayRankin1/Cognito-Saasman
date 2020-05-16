using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class ProjectUserEntityConfiguration : IEntityTypeConfiguration<ProjectUser>
    {
        public void Configure(EntityTypeBuilder<ProjectUser> builder)
        {
            builder.HasKey(ud => new { ud.ProjectId, ud.UserId });
            
            builder
                .HasOne(ud => ud.Project)
                .WithMany(d => d.Users)
                .HasForeignKey(bc => bc.ProjectId);

            builder
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(ud => ud.UserId);
        }
    }
}
