using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Restaurant.Api.Dtos;
using Restaurant.Api.Models;
using Restaurant.Api.Services;
using Restaurant.Api.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UsersController> logger;
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        // Post /user
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterUserDto userDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            User user = new()
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                Password = passwordHash,
                Role = "itemDto.Price",
                CreatedDate = DateTimeOffset.UtcNow
            };

            await userService.CreateUserAsync(user);

            return Ok();
        }
    }
}