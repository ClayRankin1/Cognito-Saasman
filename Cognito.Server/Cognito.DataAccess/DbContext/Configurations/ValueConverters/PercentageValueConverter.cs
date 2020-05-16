using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cognito.DataAccess.DbContext.Configurations.ValueConverters
{
    public class PercentageValueConverter : ValueConverter<int?, decimal?>
    {
        public PercentageValueConverter() : base(percentage => (decimal)percentage / 100, percentage => (int)(percentage * 100)) { }
    }
}
