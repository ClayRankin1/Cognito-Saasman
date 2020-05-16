using AutoMapper;
using Cognito.Business.Models;
using Cognito.Business.Services.Storage.Abstract;
using Cognito.Web.BindingModels.Common;
using Cognito.Web.BindingModels.Document;
using Cognito.Web.Infrastructure.Attributes;
using Cognito.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    [CognitoApi]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public DocumentsController(
            IMapper mapper,
            IDocumentService documentService)
        {
            _mapper = mapper;
            _documentService = documentService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDocuments([FromQuery] LocalFilterBindingModel model)
        {
            var filter = _mapper.Map<LocalFilter>(model);
            return Ok(await _documentService.GetDocumentsAsync(filter));
        }

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetDocument(int documentId) => Ok(await _documentService.GetDocumentByIdAsync(documentId));

        [HttpPost("")]
        [TransactionScope]
        public async Task<IActionResult> UploadDocument([FromQuery] DocumentBindingModel model)
        {
            if (!Request.Form.Files.Any())
            {
                ModelState.AddModelError("File", "No file was uploaded.");
                return BadRequest(ModelState);
            }

            var document = await _documentService.UploadDocumentAsync(Request.Form.Files.First(), model.TaskId.Value);

            return Ok(document);
        }

        [HttpPut("{documentId}")]
        public async Task<IActionResult> UpdateDocument(int documentId, UpdateDocumentBindingModel model)
        {
            var document = await _documentService.UpdateDocumentAsync(documentId, model.FileName, model.Description);
            return Ok(document);
        }

        [HttpPut("{documentId}/complete")]
        public async Task<IActionResult> CompleteDocument(int documentId) => Ok(await _documentService.CompleteDocumentAsync(documentId));

        [HttpPut("{documentId}/link")]
        public async Task<IActionResult> LinkDocument(int documentId, LinkDocumentBindingModel model)
        {
            await _documentService.LinkDocumentToTaskAsync(documentId, model.TaskId.Value);
            return NoContent();
        }

        [HttpDelete("{documentId}")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            await _documentService.DeleteDocumentAsync(documentId);
            return NoContent();
        }
    }
}
