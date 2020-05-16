using Cognito.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class PointDetailEntityConfiguration : IEntityTypeConfiguration<PointDetail>
    {
        public void Configure(EntityTypeBuilder<PointDetail> builder)
        {
            builder.HasKey(pp => new { pp.DetailId, pp.PointId });
            
            builder
                .HasOne(pd => pd.Point)
                .WithMany(p => p.PointDetails)
                .HasForeignKey(pd => pd.PointId);
            
            builder
                .HasOne(pd => pd.Detail)
                .WithMany(d => d.PointDetails)
                .HasForeignKey(pd => pd.DetailId);
        }
    }
}
