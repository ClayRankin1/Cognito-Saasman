//using Amazon;
//using Amazon.S3;
//using Amazon.S3.Transfer;
//using AutoMapper;
//using Cognito.Business.Services.Storage.Abstract;
//using Cognito.DataAccess.DbContext;
//using Cognito.DataAccess.Entities;
//using Cognito.Shared.Options;
//using Cognito.Web.BindingModels;
//using memento.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using System;
//using System.IO;
//using System.Threading.Tasks;

//namespace Cognito.Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [AllowAnonymous]
//    public class DocumentsOldController : ControllerBase
//    {
//        private readonly IDocumentRepository _repo;
//        private readonly IMapper _mapper;
//        private readonly UserManager<User> _userManager;
//        private readonly IOptions<AwsOptions> _awsConfig;
//        private readonly AmazonS3Client _client;
//        private readonly CognitoDbContext _context;
//        private readonly IDocumentService _documentService;

//        public DocumentsOldController(
//            CognitoDbContext context,
//            IDocumentRepository repo,
//            IMapper mapper,
//            UserManager<User> userManager,
//            IOptions<AwsOptions> awsConfig,
//            IDocumentService documentService)
//        {
//            _repo = repo;
//            _mapper = mapper;
//            _userManager = userManager;
//            _awsConfig = awsConfig;
//            _client = new AmazonS3Client(_awsConfig.Value.AccessKey, _awsConfig.Value.SecretAccessKey, RegionEndpoint.USEast1);
//            _context = context;
//            _documentService = documentService;
//        }

//        [HttpPost("DraftSaveAs")]
//        public async Task<IActionResult> DraftSaveAs([FromForm] CreateDocumentDraftBindingModel model)
//        {
//            var fileContents = Convert.FromBase64String(model.Base64);

//            var isExisting = false;

//            // TODO: Clean up below
//            model.FileName = model.FileName.Replace("https://s3.amazonaws.com/memento.static/", "");

//            model.FileName = model.FileName.Replace("%20", " ");

//            if (model.FileName.Contains("?"))
//            {
//                isExisting = true;
//                model.FileName = model.FileName.Substring(0, model.FileName.LastIndexOf("?")) + "/0";
//            }

//            var key = $"{model.FileName}";

//            var newDoc = new Document
//            {
//                FileName = model.FileName.Split('/')[4],
//                DocumentStatusId = model.Reason == "ConvertToCompleted" ? DocumentStatusId.Completed : DocumentStatusId.Draft,
//                DateAdded = DateTime.Now,
//                DateUpdated = DateTime.Now,
//                // UserId = int.Parse(model.FileName.Split('/')[1]),
//                Key = model.FileName.Substring(0, model.FileName.LastIndexOf('/'))
//            };

//            try
//            {
//                if (!isExisting)
//                {
//                    _context.Documents.Add(newDoc);
//                }

//                _context.SaveChanges();
//            }
//            catch (Exception e)
//            {

//                var errMsg = e.Message;
//            }

//            if (!isExisting)
//                // TODO: Verify fields are correctly mapped to object below
//                AddDocToItems(
//                    new CreateItemDocumentBindingModel()
//                    {
//                        DocId = newDoc.Id,
//                        DomainId = int.Parse(model.FileName.Split('/')[2]),
//                        FileName = newDoc.FileName,
//                        ActId = int.Parse(model.FileName.Split('/')[3]),
//                        MatterId = int.Parse(model.FileName.Split('/')[5])
//                    });

//            await using (var newMemoryStream = new MemoryStream(fileContents))
//            {

//                var uploadRequest = new TransferUtilityUploadRequest
//                {
//                    BucketName = _awsConfig.Value.BucketName,
//                    InputStream = newMemoryStream,
//                    Key = newDoc.Key
//                };

//                var fileTransferUtility = new TransferUtility(_client);
//                await fileTransferUtility.UploadAsync(uploadRequest);
//            }

//            return Ok();
//        }
//    }
//}