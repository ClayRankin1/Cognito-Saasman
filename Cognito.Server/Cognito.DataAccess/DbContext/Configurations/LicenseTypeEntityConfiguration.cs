using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class LicenseTypeEntityConfiguration : IEntityTypeConfiguration<LicenseType>
    {
        public void Configure(EntityTypeBuilder<LicenseType> builder)
        {
            builder
                .Property(e => e.Id)
                .HasConversion<int>();

            builder.HasData(EnumHelper.ConvertToLookup<LicenseTypeId, LicenseType>());
        }
    }
}
