using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class UserDomainEntityConfiguration : IEntityTypeConfiguration<UserDomain>
    {
        public void Configure(EntityTypeBuilder<UserDomain> builder)
        {
            builder.HasKey(ud => new { ud.UserId, ud.DomainId, ud.RoleId });

            builder
                .HasOne(ud => ud.Domain)
                .WithMany(d => d.UserDomains)
                .HasForeignKey(bc => bc.DomainId);
            
            builder
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDomains)
                .HasForeignKey(ud => ud.UserId);
        }
    }
}
