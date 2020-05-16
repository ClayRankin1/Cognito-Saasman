using AutoMapper;
using Cognito.Business.DataServices.Abstract;
using Cognito.Business.Models;
using Cognito.Business.Services.Abstract;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels;
using Cognito.Web.BindingModels.Common;
using Cognito.Web.BindingModels.Contact;
using Cognito.Web.Controllers.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    public class ContactsController : CrudControllerBase<Contact, CreateContactBindingModel, UpdateContactBindingModel, ContactViewModel, IContactDataService>
    {
        private readonly IContactService _contactService;

        public ContactsController(
            IMapper mapper,
            IContactDataService dataService,
            IContactService contactService) : base(mapper, dataService)
        {
            _contactService = contactService;
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> GetAll() => base.GetAll();

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] LocalFilterBindingModel model)
        {
            var filter = _mapper.Map<LocalFilter>(model);
            return Ok(await _dataService.GetContactsAsync(filter));
        }

        public override async Task<IActionResult> Create([FromBody] CreateContactBindingModel model)
        {
            var contact = _mapper.Map<Contact>(model);

            return Ok(await _contactService.CreateContactAsync(contact, model.TaskId.Value));
        }

        public override async Task<IActionResult> Update(int id, [FromBody] UpdateContactBindingModel model)
        {
            var contact = _mapper.Map<Contact>(model);

            contact.Id = id;

            return Ok(await _contactService.UpdateContactAsync(contact));
        }

        public override async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _contactService.DeleteContactAsync(id);
            return NoContent();
        }

        [HttpPost(nameof(Link))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Link(LinkContactBindingModel model)
        {
            await _contactService.CreateContactLinkAsync(model.ContactId.Value, model.TaskId.Value);
            return NoContent();
        }
    }
}
