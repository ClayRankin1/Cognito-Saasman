using Cognito.Business.Models;
using Cognito.Business.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Storage.Abstract
{
    public interface IDocumentService
    {
        Task<DocumentViewModel[]> GetDocumentsAsync(LocalFilter filter);

        Task<DocumentViewModel> GetDocumentByIdAsync(int documentId);

        Task<DocumentViewModel> CompleteDocumentAsync(int documentId);

        Task LinkDocumentToTaskAsync(int documentId, int taskId);

        Task<DocumentViewModel> UploadDocumentAsync(IFormFile file, int taskId);

        Task<DocumentViewModel> UpdateDocumentAsync(int documentId, string fileName, string description);

        Task DeleteDocumentAsync(int documentId);
    }
}
