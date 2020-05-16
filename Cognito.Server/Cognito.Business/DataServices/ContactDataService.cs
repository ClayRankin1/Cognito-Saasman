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
    public class ContactDataService : DataServiceBase<Contact, ContactViewModel, IContactRepository>, IContactDataService
    {
        public ContactDataService(
            IMapper mapper,
            IContactRepository repository,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService) : base(mapper, repository, dateTimeProvider, currentUserService)
        {
        }

        public Task<ContactViewModel[]> GetContactsAsync(LocalFilter filter)
        {
            return _repository
                .GetAll()
                .SelectMany(c => c.TaskContacts)
                .ApplyLocalFilter(filter)
                .Select(tc => tc.Contact)
                .ProjectTo<ContactViewModel>(_mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}
