using Cognito.DataAccess.Attributes;

namespace Cognito.DataAccess.Entities
{
    public class DomainLicense
    {
        public int DomainId { get; set; }

        public virtual Domain Domain { get; set; }

        public int LicenseId { get; set; }

        public virtual License License { get; set; }

        public int? Discount { get; set; }

        public int Licenses { get; set; }

        [DecimalPrecision(18,2)]
        public decimal Price { get; set; }
    }
}
