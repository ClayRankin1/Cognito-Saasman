using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class StatusTypeEntityConfiguration : IEntityTypeConfiguration<StatusType>
    {
        public void Configure(EntityTypeBuilder<StatusType> builder)
        {
            builder.HasData(new StatusType[]
            {
                new StatusType { Id = 1, Label = "Pending", DisplayOrder = 1 },
                new StatusType { Id = 6, Label = "Unschedule", DisplayOrder = 2 },
                new StatusType { Id = 9, Label = "Complete", DisplayOrder = 3 },
                new StatusType { Id = 14, Label = "Archived", DisplayOrder = 4 }
            });
        }
    }
}
