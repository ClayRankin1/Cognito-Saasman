using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class DocumentStatusEntityConfiguration : IEntityTypeConfiguration<DocumentStatus>
    {
        public void Configure(EntityTypeBuilder<DocumentStatus> builder)
        {
            builder
                .Property(e => e.Id)
                .HasConversion<int>();

            builder.HasData(EnumHelper.ConvertToLookup<DocumentStatusId, DocumentStatus>());
        }
    }
}
