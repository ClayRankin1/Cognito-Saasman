using Cognito.DataAccess.Entities;
using Cognito.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class TimeEntityConfiguration : IEntityTypeConfiguration<Time>
    {
        public void Configure(EntityTypeBuilder<Time> builder)
        {
            builder
                .Property(e => e.TimeTypeId)
                .HasConversion<int>();

            var start = DateTime.Today;
            var absoluteTimes = Enumerable
                .Range(0, 48)
                .Select((offset, index) => new Time
                {
                    Id = index + 1,
                    Label = start.AddMinutes(30 * offset).ToString(DateFormats.TimeFormat),
                    TimeTypeId = TimeTypeId.Absolute
                });

            var relativeTimes = new[] { "1-ASAP", "2-First", "3-Today", "4-Later", "5-Low" }
                .Select((label, index) => new Time 
                {
                    Id = (index + 1) + 48,
                    Label = label,
                    TimeTypeId = TimeTypeId.Relative
                });

            var allTimes = absoluteTimes.Concat(relativeTimes);

            builder.HasData(allTimes);
        }
    }
}
