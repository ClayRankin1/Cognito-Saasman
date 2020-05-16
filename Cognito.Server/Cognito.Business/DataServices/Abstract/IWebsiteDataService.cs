using Cognito.Business.Models;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IWebsiteDataService : IDataService<Website, WebsiteViewModel>
    {
        Task<WebsiteViewModel[]> GetWebsitesAsync(LocalFilter filter);
    }
}
