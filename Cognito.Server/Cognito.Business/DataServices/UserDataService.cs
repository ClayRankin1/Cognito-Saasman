using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Business.DataServices
{
    public class UserDataService : DataServiceBase<User, UserViewModel, IUserRepository>, IUserDataService
    {
        public UserDataService(IMapper mapper, 
            IUserRepository repository, 
            IDateTimeProvider dateTimeProvider, 
            ICurrentUserService currentUserService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersByDomainIdAsync(int id)
        {
            return await _repository.GetAll()
                .Where(u => u.UserDomains.Any(ud => ud.DomainId == id))
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
