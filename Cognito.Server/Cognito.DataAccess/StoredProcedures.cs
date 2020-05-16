namespace Cognito.DataAccess
{
    public static class StoredProcedures
    {
        public const string GetUser = "[dbo].[GetUser]";

        public const string UpdateUserRefreshToken = "[dbo].[UpdateUserRefreshToken]";

        public const string MergeSubtasks = "[dbo].[MergeSubtasks]";

        public const string MergeProjectUsers = "[dbo].[MergeProjectUsers]";

        public const string CopyTask = "[Task].[CopyTask]";
    }
}
