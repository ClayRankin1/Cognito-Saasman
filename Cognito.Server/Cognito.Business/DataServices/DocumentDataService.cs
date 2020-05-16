using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;

namespace Cognito.Business.DataServices
{
    public class DocumentDataService : DataServiceBase<Document, DocumentViewModel, IDocumentRepository>, IDocumentDataService
    {
        public DocumentDataService(
            IMapper mapper,
            IDocumentRepository repository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
        }
    }
}
