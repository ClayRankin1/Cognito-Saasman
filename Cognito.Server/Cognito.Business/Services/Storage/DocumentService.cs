using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Exceptions;
using Cognito.Business.Extensions;
using Cognito.Business.Models;
using Cognito.Business.Services.Storage.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Extensions;
using Cognito.Shared.Services.Common.Abstract;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Storage
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private readonly ITaskRepository _taskRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentDataService _documentDataService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDetailRepository _detailRepository;
        private readonly IDetailDataService _detailDataService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DocumentService(
            IMapper mapper,
            IStorageService storageService,
            ITaskRepository taskRepository,
            IDocumentRepository documentRepository,
            IDocumentDataService documentDataService,
            ICurrentUserService currentUserService,
            IDetailRepository detailRepository,
            IDetailDataService detailDataService,
            IDateTimeProvider dateTimeProvider)
        {
            _mapper = mapper;
            _storageService = storageService;
            _taskRepository = taskRepository;
            _documentRepository = documentRepository;
            _documentDataService = documentDataService;
            _currentUserService = currentUserService;
            _detailRepository = detailRepository;
            _detailDataService = detailDataService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<DocumentViewModel[]> GetDocumentsAsync(LocalFilter filter)
        {
            var documents = await _documentRepository
                .GetAll()
                .SelectMany(w => w.TaskDocuments)
                .ApplyLocalFilter(filter)
                .Select(td => td.Document)
                .ProjectTo<DocumentViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            documents.ForEach(d => 
            {
                d.Url = _storageService.GetFileUrlByKey(d.Key);
            });

            return documents;
        }

        public async Task<DocumentViewModel> GetDocumentByIdAsync(int documentId)
        {
            var document = await _documentDataService.GetAsync(documentId);

            document.Url = _storageService.GetFileUrlByKey(document.Key);

            return document;
        }

        public async Task<DocumentViewModel> CompleteDocumentAsync(int documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                throw new EntityNotFoundException($"Document was not found.");
            }

            document.DocumentStatusId = DocumentStatusId.Completed;
            _documentRepository.Update(document);

            return _mapper.Map<DocumentViewModel>(document);
        }

        public async Task<DocumentViewModel> UploadDocumentAsync(IFormFile file, int taskId)
        {
            var key = GenerateRandomFileUrl(file, taskId);
            var fileName = file.FileName;

            await _storageService.UploadFileAsync(file, key);

            var newDocument = new Document 
            {
                Key = key,
                FileName = fileName,
                DocumentStatusId = DocumentStatusId.Completed
            };

            // Link new document to the project
            newDocument.TaskDocuments.Add(new TaskDocument { TaskId = taskId });

            var document = await _documentDataService.CreateAsync(newDocument);

            document.Url = _storageService.GetFileUrlByKey(key);

            // Create detail
            await _detailDataService.CreateAsync(new Detail
            {
                TaskId = taskId,
                Body = document.FileName,
                DetailTypeId = DetailTypeId.DocReference,
                SourceId = document.Id
            });

            return document;
        }

        public async Task<DocumentViewModel> UpdateDocumentAsync(int documentId, string fileName, string description)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);

            document.FileName = fileName;
            document.Description = description;

            await _documentDataService.UpdateAsync(document);

            await _detailRepository
                .GetAll()
                .Where(d => d.DetailTypeId == DetailTypeId.DocReference && d.SourceId == document.Id)
                .BatchUpdateAsync(new Detail
                {
                    Body = document.FileName,
                    UpdatedByUserId = _currentUserService.UserId,
                    DateUpdated = _dateTimeProvider.UtcNow
                });

            return _mapper.Map<DocumentViewModel>(document);
        }

        public async Task DeleteDocumentAsync(int documentId)
        {
            var document = await _documentDataService.GetAsync(documentId);

            await _detailRepository
                .GetAll()
                .Where(d => d.DetailTypeId == DetailTypeId.DocReference && d.SourceId == documentId)
                .BatchUpdateAsync(new Detail { IsDeleted = true });

            await _storageService.DeleteFileAsync(document.Key);
            await _documentDataService.DeleteAsync(document.Id);
        }

        public Task LinkDocumentToTaskAsync(int documentId, int taskId) => _taskRepository.AddDocumentAsync(taskId, documentId);

        private string GenerateRandomFileUrl(IFormFile file, int taskId)
        {
            var fileName = $"{Path.GetRandomFileName().Replace(".", "")}{Path.GetExtension(file.FileName)}";
            return $"{taskId}/{_currentUserService.UserId}/{fileName}";
        }
    }
}
