using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Extensions;
using Cognito.Business.Models;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class WebsiteDataService : DataServiceBase<Website, WebsiteViewModel, IWebsiteRepository>, IWebsiteDataService
    {
        public WebsiteDataService(
            IMapper mapper,
            IWebsiteRepository repository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
        }

        public Task<WebsiteViewModel[]> GetWebsitesAsync(LocalFilter filter)
        {
            return _repository
                .GetAll()
                .SelectMany(w => w.TaskWebsites)
                .ApplyLocalFilter(filter)
                .Select(wr => wr.Website)
                .ProjectTo<WebsiteViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}
