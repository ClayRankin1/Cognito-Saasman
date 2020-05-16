using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices.Abstract
{
    public interface IUserDataService: IDataService<User, UserViewModel>
    {
        Task<IEnumerable<UserViewModel>> GetUsersByDomainIdAsync(int id);
    }
}
