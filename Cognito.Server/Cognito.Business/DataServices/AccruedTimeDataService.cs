using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class AccruedTimeDataService : DataServiceBase<AccruedTime, AccruedTimeViewModel, IAccruedTimeRepository>, IAccruedTimeDataService
    {
        public AccruedTimeDataService(
            IMapper mapper,
            IAccruedTimeRepository repository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
        }

        public override Task<AccruedTimeViewModel> CreateAsync(AccruedTime entity)
        {
            entity.UserId = _currentUserService.UserId;
            // TODO: FIXME - Do we want to store it when we have From and To values???
            entity.Total = (entity.To - entity.From).Seconds;

            return base.CreateAsync(entity);
        }
    }
}
