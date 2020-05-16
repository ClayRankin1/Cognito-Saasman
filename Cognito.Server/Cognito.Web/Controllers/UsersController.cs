using AutoMapper;
using Cognito.DataAccess.Entities;
using Cognito.Web.BindingModels;
using Cognito.Web.Services.Abstract;
using Cognito.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cognito.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UsersController(
            IMapper mapper,
            IUserService userService,
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            // todo: FIXME - Include does not work and generates second db query
            var users = await _userManager.Users
                .Include(user => user.UserDomains)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<UserViewModel>>(users));
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userManager.Users
                .Include(u => u.UserDomains)
                .Where(u => u.Id == id)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            var userToReturn = _mapper.Map<UserViewModel>(user);
            userToReturn.DomainId = user.UserDomains.First().DomainId;

            return Ok(userToReturn);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateUser(CreateUserBindingModel model)
        {
            var result = await _userService.CreateUserAsync(model.Email, model.DomainId.Value);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                return CreatedAtAction(nameof(GetUser), new { id = result.User.Id }, _mapper.Map<UserViewModel>(result.User));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}