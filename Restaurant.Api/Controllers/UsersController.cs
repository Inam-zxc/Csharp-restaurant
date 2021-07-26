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
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Restaurant.Api.Settings;
using System.Security.Claims;
using Restaurant.Api.Constants;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UsersController> logger;

        private readonly IConfiguration config;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IConfiguration config)
        {
            this.userService = userService;
            this.logger = logger;
            this.config = config;
        }

        // Post /user
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterUserDto userDto)
        {
            var duplicateUsername = await userService.GetUserByUsernameAsync(userDto.Username);
            if (!(duplicateUsername is null))
            {
                return BadRequest("Username has been used.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            User user = new()
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                Password = passwordHash,
                Role = Constants.Constants.Roles.User,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await userService.CreateUserAsync(user);

            return Ok();
        }

        // Post /user
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginUserDto userDto)
        {
            var user = await userService.GetUserByUsernameAsync(userDto.Username);
            string errorString = $"Username doesn't exist or wrong password.";
            if (user is null)
            {
                return BadRequest(errorString);
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                return BadRequest(errorString);
            }

            var tokenString = GenerateJWT(user);

            return Ok(new { token = tokenString });
        }

        private string GenerateJWT(User user)
        {
            var jwtSettings = config.GetSection("JWT").Get<JWT>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(CustomClaimsType.UserId, user.Id.ToString()),
                new Claim(CustomClaimsType.Username, user.Username),
                new Claim(CustomClaimsType.Roles, user.Role)
            };

            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Issuer,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}