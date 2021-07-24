using Restaurant.Api.Dtos;
using Restaurant.Api.Models;

namespace Restaurant.Api
{
    public static class Extensions
    {
        public static UserDto AsUserDto(this User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Role = user.Role,
                CreatedDate = user.CreatedDate

            };
        }
    }
}