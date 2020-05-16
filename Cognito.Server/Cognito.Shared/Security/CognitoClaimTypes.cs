namespace Cognito.Shared.Security
{
    public static class CognitoClaimTypes
    {
        private static readonly string ClaimPrefix = "Memento";

        public static string DomainId = $"{ClaimPrefix}{nameof(DomainId)}".ToLower();

        public static string UserName = $"{ClaimPrefix}{nameof(UserName)}".ToLower();

        public static string DomainRoles = $"{ClaimPrefix}{nameof(DomainRoles)}".ToLower();
    }
}
