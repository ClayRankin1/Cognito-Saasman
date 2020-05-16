using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            builder
                .HasMany(t => t.Documents)
                .WithOne(g => g.CreatedByUser)
                .HasForeignKey(g => g.CreatedByUserId)
                .IsRequired();

            builder
                .HasMany(t => t.UpdatedDocuments)
                .WithOne(g => g.UpdatedByUser)
                .HasForeignKey(g => g.UpdatedByUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(t => t.UserTasks)
                .WithOne(t => t.Owner)
                .HasForeignKey(t => t.OwnerId);
        }
    }
}
