using System;

namespace Cognito.Shared.Helpers
{
    public static class AccruedHelper
    {
        private static readonly int MinutesPerHour = 60;

        public static decimal? ConvertToNumber(string input)
        {
            if (input != null)
            {
                var timeParts = input.Split(":");
                if (timeParts.Length == 2)
                {
                    return decimal.Parse(timeParts[0]) + decimal.Parse(timeParts[1]) / MinutesPerHour;
                }
            }

            return null;
        }

        public static string ConvertToString(decimal? accrued)
        {
            if (accrued.HasValue)
            {
                var wholeDigits = Math.Floor(accrued.Value);
                var fraction = accrued.Value - wholeDigits;
                return $"{wholeDigits}:{(int)(fraction * MinutesPerHour)}";
            }

            return null;
        }
    }
}
