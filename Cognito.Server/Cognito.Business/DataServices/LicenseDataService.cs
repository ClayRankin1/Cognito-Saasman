using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;

namespace Cognito.Business.DataServices
{
    public class LicenseDataService : DataServiceBase<License, LicenseViewModel, ILicenseRepository>, ILicenseDataService
    {
        public LicenseDataService(IMapper mapper, ILicenseRepository repository, IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
        }
    }
}
