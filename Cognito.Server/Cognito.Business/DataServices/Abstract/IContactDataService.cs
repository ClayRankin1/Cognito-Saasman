using Cognito.Business.Models;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IContactDataService : IDataService<Contact, ContactViewModel>
    {
        Task<ContactViewModel[]> GetContactsAsync(LocalFilter filter);
    }
}
