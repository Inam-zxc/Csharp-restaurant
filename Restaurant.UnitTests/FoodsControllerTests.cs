using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Api.Controllers;
using Restaurant.Api.Dtos;
using Restaurant.Api.Models;
using Restaurant.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Restaurant.Api.Services.Interface;
using Restaurant.Api.Constants;
using Microsoft.Extensions.Configuration;
using Restaurant.Api.Settings;

namespace Restaurant.UnitTests
{
    public class FoodsControllerTests
    {
        private readonly Mock<IFoodService> foodServiceStub = new();
        private readonly Mock<ILogger<FoodsController>> loggerStub = new();
        private readonly Mock<IConfiguration> configStub = new();
        private readonly Random rand = new();


        [Fact]
        public async Task GetAllFoodAsync_WithNoSearch_ReturnsAllItems()
        {
            // Arrange
            var expectedFoods = new[] { CreateRandomFood(), CreateRandomFood(), CreateRandomFood() };
            foodServiceStub.Setup(u => u.GetAllFoodsAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedFoods);

            var controller = new FoodsController(foodServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act
            var result = await controller.GetAllFoodAsync(string.Empty);

            // Assert
            result.Should().BeEquivalentTo(expectedFoods);
        }

        [Fact]
        public async Task GetFoodByIdsync_WithUnExistingItem_ReturnsNotFound()
        {
            // Arrange
            foodServiceStub.Setup(u => u.GetFoodByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Food)null);

            var controller = new FoodsController(foodServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act
            var result = await controller.GetFoodByIdAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetFoodByIdsync_WithExistingItem_ReturnsNotFound()
        {
            // Arrange
            var existingFood = CreateRandomFood();
            foodServiceStub.Setup(u => u.GetFoodByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingFood);

            var controller = new FoodsController(foodServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act
            var result = await controller.GetFoodByIdAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(existingFood);
        }

        [Fact]
        public async Task CreateFoodAsync_WithFoodToCreate_ReturnsCreatedItems()
        {
            // Arrange
            var itemToCreate = new CreateFoodDto(
                Guid.NewGuid().ToString(),
                rand.Next(1000));


            var controller = new FoodsController(foodServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act
            var result = await controller.CreateFoodAsync(itemToCreate);

            // Assert
            var createdFood = (result.Result as CreatedAtActionResult).Value as FoodDto;
            itemToCreate.Should().BeEquivalentTo(
                createdFood,
                options => options.ComparingByMembers<FoodDto>().ExcludingMissingMembers()
            );
            createdFood.Id.Should().NotBeEmpty();
            createdFood.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }

        [Fact]
        public async Task UpdateFoodAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var exsitingFood = CreateRandomFood();

            foodServiceStub.Setup(repo => repo.GetFoodByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(exsitingFood);

            var foodId = exsitingFood.Id;
            var foodToUpdate = new UpdateFoodDto(
                Guid.NewGuid().ToString(),
                exsitingFood.Price + 3);

            var controller = new FoodsController(foodServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act 
            var result = await controller.UpdateFoodAsync(foodId, foodToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteFoodAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var exsitingFood = CreateRandomFood();

            foodServiceStub.Setup(repo => repo.GetFoodByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(exsitingFood);


            var controller = new FoodsController(foodServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act 
            var result = await controller.DeleteFoodAsync(exsitingFood.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private RegisterUserDto CreateRandomRegister()
        {
            return new()
            {
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
        }

        private Food CreateRandomFood()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow,
                Reviews = null
            };
        }
    }
}
