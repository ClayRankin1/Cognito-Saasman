using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Abstract
{
    public interface IWebsiteService
    {
        Task<WebsiteViewModel> CreateWebsiteAsync(Website entity, int taskId);

        Task<WebsiteViewModel> UpdateWebsiteAsync(Website entity);

        Task DeleteWebsiteAsync(int id);
    }
}
