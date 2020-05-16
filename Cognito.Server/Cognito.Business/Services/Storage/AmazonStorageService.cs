using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Cognito.Business.Services.Storage.Abstract;
using Cognito.Shared.Options;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Storage
{
    public class AmazonStorageService : IStorageService
    {
        private readonly IAmazonS3 _amazonClient;
        private readonly IOptions<AwsOptions> _awsConfig;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AmazonStorageService(
            IAmazonS3 amazonClient,
            IOptions<AwsOptions> awsConfig,
            IDateTimeProvider dateTimeProvider)
        {
            _awsConfig = awsConfig;
            _amazonClient = amazonClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public string GetFileUrlByKey(string fileKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _awsConfig.Value.BucketName,
                Key = fileKey,
                Expires = _dateTimeProvider.Now.AddMinutes(_awsConfig.Value.UrlLinkExpirationInMinutes)
            };

            return _amazonClient.GetPreSignedURL(request);
        }

        public async Task UploadFileAsync(IFormFile file, string key)
        {
            await using var ms = new MemoryStream();
            file.CopyTo(ms);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                BucketName = _awsConfig.Value.BucketName,
                InputStream = ms,
                Key = key
            };

            await new TransferUtility(_amazonClient).UploadAsync(uploadRequest);
        }

        public async Task DeleteFileAsync(string key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _awsConfig.Value.BucketName,
                Key = key
            };

            await _amazonClient.DeleteObjectAsync(request);
        }
    }
}
