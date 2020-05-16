using Cognito.DataAccess.DbContext.Configurations.ValueConverters;
using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class DomainLicenseEntityConfiguration : IEntityTypeConfiguration<DomainLicense>
    {
        public void Configure(EntityTypeBuilder<DomainLicense> builder)
        {
            builder.HasKey(dl => new { dl.DomainId, dl.LicenseId });
            
            builder
                .HasOne(dl => dl.Domain)
                .WithMany(d => d.DomainLicenses)
                .HasForeignKey(dl => dl.DomainId);
            
            builder
                .HasOne(dl => dl.License)
                .WithMany(u => u.DomainLicenses)
                .HasForeignKey(dl => dl.LicenseId);

            builder
                .Property(d => d.Discount)
                .HasConversion(new PercentageValueConverter());
        }
    }
}
