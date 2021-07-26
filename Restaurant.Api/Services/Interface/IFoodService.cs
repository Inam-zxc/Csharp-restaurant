using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Api.Models;

namespace Restaurant.Api.Services.Interface
{
    public interface IFoodService
    {
        Task CreateFoodAsync(Food food);
        Task<IEnumerable<Food>> GetAllFoodsAsync(string name = null);
        Task DeleteFoodAsync(Guid id);
        Task<Food> GetFoodByIdAsync(Guid id);
        Task UpdatedFoodAsync(Food food);
    }
}