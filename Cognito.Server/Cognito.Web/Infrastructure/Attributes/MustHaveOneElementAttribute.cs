using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Cognito.Web.Infrastructure.Attributes
{
    public sealed class MustHaveOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IEnumerable collection && collection.GetEnumerator().MoveNext())
            {
                return true;
            }

            return false;
        }
    }
}
