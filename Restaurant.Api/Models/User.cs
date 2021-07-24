using System;

namespace Restaurant.Api.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}