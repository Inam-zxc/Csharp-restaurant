using System;

namespace Restaurant.Api.Dtos
{
    public class UserDto
    {
        public Guid Id { get; init; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}