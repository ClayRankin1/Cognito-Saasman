using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Storage.Abstract
{
    public interface IStorageService
    {
        string GetFileUrlByKey(string fileKey);

        Task UploadFileAsync(IFormFile file, string key);

        Task DeleteFileAsync(string key);
    }
}
