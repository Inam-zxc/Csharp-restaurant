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
using Restaurant.Api.Constants;

namespace Restaurant.Api.Controllers
{
    [ApiController]
    [Route("reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IFoodService foodService;
        private readonly ILogger<FoodsController> logger;

        private readonly IConfiguration config;

        public ReviewsController(IFoodService foodService, ILogger<FoodsController> logger, IConfiguration config)
        {
            this.foodService = foodService;
            this.logger = logger;
            this.config = config;
        }

        // @desc    create review
        // @route   POST /reviews/food/{id}
        [HttpPost("food/{id}")]
        [Authorize]
        [ClaimRequirement(CustomClaimsType.Roles, Constants.Constants.Roles.User)]
        public async Task<ActionResult<FoodWithReviewsDto>> CreateReviewAsync(Guid id, CreateReviewDto reviewDto)
        {

            var existingFood = await foodService.GetFoodByIdAsync(id);
            if (existingFood is null)
            {
                return BadRequest("We don't have food with that id.");
            }
            var currentUser = HttpContext.User;
            var username = currentUser.Claims.Where(c => c.Type == CustomClaimsType.Username).Select(c => c.Value).SingleOrDefault();

            if (existingFood.Reviews != null)
            {
                var existingReview = existingFood.Reviews.Find(c => c.User == username);
                if (existingReview != null)
                {
                    return BadRequest("This food is already reviewd by you!");
                }
            }

            Review review = new()
            {
                Id = Guid.NewGuid(),
                User = username,
                Star = reviewDto.Star,
                Description = reviewDto.Description,
                CreatedDate = DateTimeOffset.UtcNow,
                LatestEditedDate = DateTimeOffset.Now
            };
            List<Review> newReviews = new();
            if (existingFood.Reviews != null)
            {
                newReviews = existingFood.Reviews;

            }
            newReviews.Add(review);
            existingFood.Reviews = newReviews;

            await foodService.UpdatedFoodAsync(existingFood);
            return CreatedAtAction(nameof(FoodsController.GetFoodByIdAsync), "Foods", new { id = existingFood.Id }, existingFood.AsFoodWithReviewsDto());
        }

        // @desc    update review
        // @route   POST /reviews/food/{id}
        [HttpPut("food/{id}")]
        [Authorize]
        [ClaimRequirement(CustomClaimsType.Roles, Constants.Constants.Roles.User)]
        public async Task<ActionResult<FoodWithReviewsDto>> UpdateReviewAsync(Guid id, UpdateReviewDto reviewDto)
        {

            var existingFood = await foodService.GetFoodByIdAsync(id);
            if (existingFood is null)
            {
                return BadRequest("We don't have food with that id.");
            }
            var currentUser = HttpContext.User;
            var username = currentUser.Claims.Where(c => c.Type == CustomClaimsType.Username).Select(c => c.Value).SingleOrDefault();

            if (existingFood.Reviews != null)
            {
                var existingReview = existingFood.Reviews.Find(c => c.User == username);
                if (existingReview is null)
                {
                    return BadRequest("This food is never reviewed by you!");
                }

                Review review = new()
                {
                    Id = existingReview.Id,
                    User = username,
                    Star = reviewDto.Star,
                    Description = reviewDto.Description,
                    CreatedDate = existingReview.CreatedDate,
                    LatestEditedDate = DateTimeOffset.UtcNow
                };
                List<Review> newReviews = new();
                if (existingFood.Reviews != null)
                {
                    newReviews = existingFood.Reviews;

                }
                newReviews[newReviews.FindIndex(r => r.Id == existingReview.Id)] = review;
                existingFood.Reviews = newReviews;

                await foodService.UpdatedFoodAsync(existingFood);
                return CreatedAtAction(nameof(FoodsController.GetFoodByIdAsync), "Foods", new { id = existingFood.Id }, existingFood.AsFoodWithReviewsDto());
            }
            return BadRequest("This food doesn't have review.");
        }

        // @desc    update review
        // @route   POST /reviews/food/{id}
        [HttpDelete("food/{id}")]
        [Authorize]
        [ClaimRequirement(CustomClaimsType.Roles, Constants.Constants.Roles.User)]
        public async Task<ActionResult<FoodWithReviewsDto>> DeleteReviewAsync(Guid id)
        {

            var existingFood = await foodService.GetFoodByIdAsync(id);
            if (existingFood is null)
            {
                return BadRequest("We don't have food with that id.");
            }
            var currentUser = HttpContext.User;
            var username = currentUser.Claims.Where(c => c.Type == CustomClaimsType.Username).Select(c => c.Value).SingleOrDefault();

            if (existingFood.Reviews != null)
            {
                var existingReview = existingFood.Reviews.Find(c => c.User == username);
                if (existingReview is null)
                {
                    return BadRequest("This food is never reviewed by you!");
                }


                List<Review> newReviews = new();
                if (existingFood.Reviews != null)
                {
                    newReviews = existingFood.Reviews;

                }
                newReviews.RemoveAt(newReviews.FindIndex(r => r.Id == existingReview.Id));
                existingFood.Reviews = newReviews;

                await foodService.UpdatedFoodAsync(existingFood);
                return CreatedAtAction(nameof(FoodsController.GetFoodByIdAsync), "Foods", new { id = existingFood.Id }, existingFood.AsFoodWithReviewsDto());

            }
            return BadRequest("This food doesn't have review.");
        }

    }
}