using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class TimeTypeEntityConfiguration : IEntityTypeConfiguration<TimeType>
    {
        public void Configure(EntityTypeBuilder<TimeType> builder)
        {
            builder
                .Property(e => e.Id)
                .HasConversion<int>();

            builder.HasData(EnumHelper.ConvertToLookup<TimeTypeId, TimeType>());
        }
    }
}
