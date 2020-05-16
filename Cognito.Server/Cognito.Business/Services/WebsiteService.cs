using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using EFCore.BulkExtensions;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.Services
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebsiteDataService _websiteDataService;
        private readonly IDetailRepository _detailRepository;
        private readonly IDetailDataService _detailDataService;

        public WebsiteService(
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IWebsiteDataService websiteDataService,
            IDetailRepository detailRepository,
            IDetailDataService detailDataService)
        {
            _dateTimeProvider = dateTimeProvider;
            _currentUserService = currentUserService;
            _websiteDataService = websiteDataService;
            _detailRepository = detailRepository;
            _detailDataService = detailDataService;
        }

        public async Task<WebsiteViewModel> CreateWebsiteAsync(Website entity, int taskId)
        {
            entity.TaskWebsites.Add(new TaskWebsite
            {
                TaskId = taskId
            });

            var website = await _websiteDataService.CreateAsync(entity);

            // Create detail
            await _detailDataService.CreateAsync(new Detail
            {
                TaskId = taskId,
                Body = website.Url,
                DetailTypeId = DetailTypeId.WebReference,
                SourceId = website.Id
            });

            return website;
        }

        public async Task<WebsiteViewModel> UpdateWebsiteAsync(Website entity)
        {
            var website = await _websiteDataService.UpdateAsync(entity);

            // Bulk update all the Details Body properties for this website reference
            await _detailRepository
                .GetAll()
                .Where(d => d.DetailTypeId == DetailTypeId.WebReference && d.SourceId == website.Id)
                .BatchUpdateAsync(new Detail 
                { 
                    Body = website.Url,
                    UpdatedByUserId = _currentUserService.UserId,
                    DateUpdated = _dateTimeProvider.UtcNow
                });

            return website;
        }

        public async Task DeleteWebsiteAsync(int websiteId)
        {
            await _detailRepository
                .GetAll()
                .Where(d => d.DetailTypeId == DetailTypeId.WebReference && d.SourceId == websiteId)
                .BatchUpdateAsync(new Detail { IsDeleted = true });

            await _websiteDataService.DeleteAsync(websiteId);
        }
    }
}
