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
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> userServiceStub = new();
        private readonly Mock<ILogger<UsersController>> loggerStub = new();
        private readonly Mock<IConfiguration> configStub = new();

        [Fact]
        public async Task RegisterAsync_WithNewUsername_ReturnOk()
        {
            // Arrange
            userServiceStub.Setup(u => u.GetUserByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new UsersController(userServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act
            var result = await controller.RegisterAsync(CreateRandomRegister());

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RegisterAsync_WithDulpicatedUsername_ReturnBadRequest()
        {
            // Arrange
            var dulpicatedUser = CreateRandomUser();
            userServiceStub.Setup(u => u.GetUserByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync(dulpicatedUser);

            var controller = new UsersController(userServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act
            var result = await controller.RegisterAsync(CreateRandomRegister());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task LoginAsync_WithNoUsername_ReturnBadRequest()
        {
            // Arrange
            var loginForm = new LoginUserDto
            {
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
            var dulpicatedUser = CreateRandomUser();
            userServiceStub.Setup(u => u.GetUserByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var controller = new UsersController(userServiceStub.Object, loggerStub.Object, configStub.Object);

            // Act
            var result = await controller.LoginAsync(loginForm);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        // [Fact]
        // public async Task LoginAsync_WithRealUsernameAndRightPassword_ReturnOkAndToken()
        // {

        //     // Arrange
        //     LoginUserDto loginForm = new()
        //     {
        //         Username = "Test",
        //         Password = "Test1234"
        //     };

        //     User mockUser = new()
        //     {
        //         Id = Guid.NewGuid(),
        //         Username = loginForm.Username,
        //         Password = BCrypt.Net.BCrypt.HashPassword(loginForm.Password),
        //         CreatedDate = DateTimeOffset.UtcNow,
        //         Role = Constants.Roles.User
        //     };
        //     var mockConfSection = new Mock<IConfigurationSection>();
        //     mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "Key")]).Returns("KeyOrSecretOrMcDonald");
        //     mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "Issuer")]).Returns("Food");


        //     configStub.Setup(a => a.GetSection(It.Is<string>(s => s == "JWT"))).Returns(mockConfSection.Object);

        //     // configStub.SetupGet(x => x[It.Is<string>(s => s == "JWT")]).Returns("KeyOrSecretOrMcDonald");
        //     // configStub.SetupGet(x => x[It.Is<string>(s => s == "JWT:Issuer")]).Returns("Food.Breakfast");

        //     userServiceStub.Setup(u => u.GetUserByUsernameAsync(It.Is<string>(s => s == "Test")))
        //         .ReturnsAsync(mockUser);

        //     var controller = new UsersController(userServiceStub.Object, loggerStub.Object, configStub.Object);

        //     // Act
        //     var result = await controller.LoginAsync(loginForm);

        //     // Assert
        //     result.Should().BeOfType<OkObjectResult>();
        // }



        private RegisterUserDto CreateRandomRegister()
        {
            return new()
            {
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
        }

        private User CreateRandomUser()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                Role = Constants.Roles.User,
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}
