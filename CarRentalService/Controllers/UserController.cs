using CarRentalService.Data;
using CarRentalService.Models;
using CarRentalService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext context;

        public UserController(IUserService userService, IUserRepository userRepository, AppDbContext context)
        {
            _userService = userService;
            _userRepository = userRepository;
            this.context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProducts()
        {
            var res = context.Users.ToList();
            return Ok(res);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!await _userService.RegisterUser(user))
                return BadRequest("User already exists.");

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User u)
        {
            var token = await _userService.AuthenticateUser(u.Email, u.Password);
            if (token == null)
                return Unauthorized("Invalid credentials.");

            return Ok(new { Token = token });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userDeleted = await _userRepository.DeleteUser(id);
            if (!userDeleted)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return Ok($"User with ID {id} has been deleted.");
        }
    }
}
