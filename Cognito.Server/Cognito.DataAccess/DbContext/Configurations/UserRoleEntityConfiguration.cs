using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder
                .HasOne(ur => ur.User)
                .WithMany(l => l.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            builder
                .HasOne(ur => ur.Role)
                .WithMany(l => l.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}
