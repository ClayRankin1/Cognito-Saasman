using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System.Threading.Tasks;

namespace Cognito.Business.Services.Abstract
{
    public interface IContactService
    {
        Task<ContactViewModel> CreateContactAsync(Contact entity, int taskId);

        Task<ContactViewModel> UpdateContactAsync(Contact entity);

        Task DeleteContactAsync(int contactId);

        Task CreateContactLinkAsync(int contactId, int taskId);
    }
}
