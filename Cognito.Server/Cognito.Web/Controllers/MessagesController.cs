using AutoMapper;
using Cognito.Business.ViewModels;
using Cognito.DataAccess.Entities;
using Cognito.DataAccess.Repositories.Abstract;
using Cognito.Shared.Services.Common.Abstract;
using Cognito.Web.BindingModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _repository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly UserManager<User> _userManager;

        public MessagesController(
            IMapper mapper,
            IMessageRepository repository,
            IDateTimeProvider dateTimeProvider,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userManager;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var messageFromRepo = await _repository.GetByIdAsync(id);

            if (messageFromRepo == null)
                return NotFound();

            if (messageFromRepo.SenderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(messageFromRepo);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetMessagesForUser([FromQuery]MessageParams messageParams)
        //{
        //    messageParams.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    var messagesFromRepo = await _repository.GetMessagesForUser(messageParams);

        //    var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

        //    Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

        //    return Ok(messages);
        //}

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int recipientId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var messageFromRepo = await _repository.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageViewModel>>(messageFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageBindingModel model)
        {
            model.SenderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var recipient = await _userManager.Users.Where(u => u.Id == model.RecipientId).SingleOrDefaultAsync();

            if (recipient == null)
            {
                return BadRequest();
            }

            var message = _mapper.Map<Message>(model);

            message.DateSent = _dateTimeProvider.UtcNow;

            _repository.Add(message);

            var messageToReturn = _mapper.Map<MessageViewModel>(message);

            if (await _repository.SaveAllAsync())
            {
                return CreatedAtRoute(nameof(GetMessage), new { id = message.Id }, messageToReturn);
            }
                
            // TODO: Mode to exception handling middleware
            throw new Exception("Creating the message failed on save.");
        }
    }
}