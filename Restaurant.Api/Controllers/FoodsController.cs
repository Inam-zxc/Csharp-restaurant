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
using Microsoft.AspNetCore.Authorization;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("foods")]
    public class FoodsController : ControllerBase
    {
        private readonly IFoodService foodService;
        private readonly ILogger<FoodsController> logger;

        private readonly IConfiguration config;

        public FoodsController(IFoodService foodService, ILogger<FoodsController> logger, IConfiguration config)
        {
            this.foodService = foodService;
            this.logger = logger;
            this.config = config;
        }

        // @desc    get all foods
        // @route   GET /foods
        [HttpGet]
        public async Task<IEnumerable<FoodDto>> GetAllFoodAsync(string search = null)
        {
            var foods = (await foodService.GetAllFoodsAsync(search)).Select(food => food.AsFoodDto());
            return foods;
        }

        // @desc    get food by id
        // @route   GET /foods/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodDto>> GetFoodByIdAsync(Guid id)
        {
            var food = await foodService.GetFoodByIdAsync(id);

            if (food is null)
            {
                return NotFound();
            }

            return food.AsFoodDto();
        }

        // @desc    create food
        // @route   POST /foods
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<FoodDto>> CreateFoodAsync(CreateFoodDto foodDto)
        {
            Food food = new()
            {
                Id = Guid.NewGuid(),
                Name = foodDto.Name,
                Price = foodDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
                Reviews = null
            };

            await foodService.CreateFoodAsync(food);
            return CreatedAtAction(nameof(GetFoodByIdAsync), new { id = food.Id }, food.AsFoodDto());
        }

        // @desc    update food
        // @route   PUT /foods/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateFoodAsync(Guid id, UpdateFoodDto itemDto)
        {
            var existingFood = await foodService.GetFoodByIdAsync(id);

            if (existingFood is null)
            {
                return NotFound();
            }

            existingFood.Name = itemDto.Name;
            existingFood.Price = itemDto.Price;

            await foodService.UpdatedFoodAsync(existingFood);
            return NoContent();
        }

        // @desc    delete food
        // @route   DELETE /foods/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteFoodAsync(Guid id)
        {
            var existingFood = await foodService.GetFoodByIdAsync(id);

            if (existingFood is null)
            {
                return NotFound();
            }

            await foodService.DeleteFoodAsync(existingFood.Id);
            return NoContent();
        }
    }
}