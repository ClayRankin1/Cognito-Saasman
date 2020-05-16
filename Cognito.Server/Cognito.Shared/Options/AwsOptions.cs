namespace Cognito.Shared.Options
{
    public class AwsOptions
    {
        public string BucketName { get; set; }

        public string AccessKey { get; set; }
        
        public string SecretAccessKey { get; set; }
        
        public string WebmasterEmail { get; set; }

        public int UrlLinkExpirationInMinutes { get; set; }
    }
}