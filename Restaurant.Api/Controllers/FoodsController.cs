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
        private readonly IUserService userService;
        private readonly ILogger<UsersController> logger;

        private readonly IConfiguration config;

        public FoodsController(IUserService userService, ILogger<UsersController> logger, IConfiguration config)
        {
            this.userService = userService;
            this.logger = logger;
            this.config = config;
        }

        // Get /test
        [HttpGet("Test")]
        public ActionResult Test()
        {

            return Ok("IT's just a test");
        }

        // Get /test
        [HttpGet("auth")]
        [Authorize]
        public ActionResult Auth()
        {

            return Ok("IT's just a test");
        }
    }
}