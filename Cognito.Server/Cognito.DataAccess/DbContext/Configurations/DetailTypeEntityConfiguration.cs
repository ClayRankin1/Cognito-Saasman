using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Helpers;
using Cognito.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace Cognito.DataAccess.DbContext.Configurations
{
    internal class DetailTypeEntityConfiguration : IEntityTypeConfiguration<DetailType>
    {
        private readonly DetailTypeId[] SystemDetailTypes = 
        {
            DetailTypeId.Quote,
            DetailTypeId.WebReference,
            DetailTypeId.ContactReference,
            DetailTypeId.Email,
            DetailTypeId.DocReference
        };

        public void Configure(EntityTypeBuilder<DetailType> builder)
        {
            builder
                .Property(e => e.Id)
                .HasConversion<int>();

            var detailTypes = EnumHelper
                .ConvertToLookup<DetailTypeId, DetailType>()
                .ForEach(dt => 
                {
                    dt.DetailTypeSourceTypeId = SystemDetailTypes.Contains(dt.Id)
                        ? DetailTypeSourceTypeId.System
                        : DetailTypeSourceTypeId.User;

                    return dt;
                });


            builder.HasData(detailTypes);
        }
    }
}
