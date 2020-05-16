using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.Infrastructure.Attributes
{
    public class ZipCodeAttribute : RegularExpressionAttribute
    {
        public ZipCodeAttribute() : base(@"^\d{5}$") { }
    }
}
