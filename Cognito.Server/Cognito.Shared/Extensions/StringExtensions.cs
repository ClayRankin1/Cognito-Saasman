using Pluralize.NET;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cognito.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string Pluralize(this string input) => new Pluralizer().Pluralize(input);

        public static string InsertSpacesBeforeCapitals(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var words = Regex.Matches(input, @"([A-Z][a-z]+)")
                .Cast<Match>()
                .Select(m => m.Value);

            return string.Join(" ", words);
        }
    }
}
