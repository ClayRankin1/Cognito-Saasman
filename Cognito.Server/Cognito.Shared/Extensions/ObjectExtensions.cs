namespace Cognito.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static int ParseIntSafe(this object input) =>
            int.TryParse(input?.ToString() ?? string.Empty, out var result)
                ? result
                : default;
    }
}
