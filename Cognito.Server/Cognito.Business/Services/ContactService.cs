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
    public class ContactService : IContactService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IContactDataService _contactDataService;
        private readonly ITaskRepository _taskRepository;
        private readonly IDetailRepository _detailRepository;
        private readonly IDetailDataService _detailDataService;

        public ContactService(
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IContactDataService contactDataService,
            ITaskRepository taskRepository,
            IDetailRepository detailRepository,
            IDetailDataService detailDataService)
        {
            _dateTimeProvider = dateTimeProvider;
            _currentUserService = currentUserService;
            _contactDataService = contactDataService;
            _taskRepository = taskRepository;
            _detailRepository = detailRepository;
            _detailDataService = detailDataService;
        }

        public async Task<ContactViewModel> CreateContactAsync(Contact entity, int taskId)
        {
            entity.TaskContacts.Add(new TaskContact
            {
                TaskId = taskId
            });

            var contact = await _contactDataService.CreateAsync(entity);

            // Create detail
            await _detailDataService.CreateAsync(new Detail
            {
                TaskId = taskId,
                Body = GetContactBody(contact),
                DetailTypeId = DetailTypeId.ContactReference,
                SourceId = contact.Id
            });

            return contact;
        }

        public async Task<ContactViewModel> UpdateContactAsync(Contact entity)
        {
            var contact = await _contactDataService.UpdateAsync(entity);

            // Bulk update all the Details Body properties for this contact reference
            await _detailRepository
                .GetAll()
                .Where(d => d.DetailTypeId == DetailTypeId.WebReference && d.SourceId == contact.Id)
                .BatchUpdateAsync(new Detail
                {
                    Body = GetContactBody(contact),
                    UpdatedByUserId = _currentUserService.UserId,
                    DateUpdated = _dateTimeProvider.UtcNow
                });

            return contact;
        }

        public async Task DeleteContactAsync(int contactId)
        {
            await _detailRepository
                .GetAll()
                .Where(d => d.DetailTypeId == DetailTypeId.WebReference && d.SourceId == contactId)
                .BatchUpdateAsync(new Detail { IsDeleted = true });

            await _contactDataService.DeleteAsync(contactId);
        }

        public Task CreateContactLinkAsync(int contactId, int taskId) => _taskRepository.AddContactAsync(taskId, contactId);

        private string GetContactBody(ContactViewModel contact)
        {
            // TODO: FIXME - Change according to future spec
            return $"{contact.FirstName} ${contact.LastName}";
        }
    }
}
