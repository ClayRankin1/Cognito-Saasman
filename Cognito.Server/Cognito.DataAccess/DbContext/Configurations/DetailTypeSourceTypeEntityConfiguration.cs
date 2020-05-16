using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class DetailTypeSourceTypeEntityConfiguration : IEntityTypeConfiguration<DetailTypeSourceType>
    {
        public void Configure(EntityTypeBuilder<DetailTypeSourceType> builder)
        {
            builder
                .Property(e => e.Id)
                .HasConversion<int>();

            builder.HasData(EnumHelper.ConvertToLookup<DetailTypeSourceTypeId, DetailTypeSourceType>());
        }
    }
}
