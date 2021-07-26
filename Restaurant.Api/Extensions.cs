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

        public static FoodDto AsFoodDto(this Food food)
        {
            return new FoodDto(food.Id, food.Name, food.Price, food.CreatedDate);
        }

        public static FoodWithReviewsDto AsFoodWithReviewsDto(this Food food)
        {
            return new FoodWithReviewsDto(food.Id, food.Name, food.Price, food.CreatedDate, food.Reviews);
        }
    }
}