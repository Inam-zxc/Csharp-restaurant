using System;
using System.Collections.Generic;
using Restaurant.Api.Models;

namespace Restaurant.Api.Dtos
{
    public record FoodDto(Guid Id, string Name, decimal Price, DateTimeOffset CreatedDate, List<Review> Reviews);
    public record CreateFoodDto(string Name, decimal Price);
    public record UpdateFoodDto(string Name, decimal Price);
}