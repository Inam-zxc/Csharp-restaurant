using System;
using System.Threading.Tasks;
using Restaurant.Api.Models;

namespace Restaurant.Api.Services.Interface
{
    public interface IUserService
    {
        Task CreateUserAsync(User user);
    }
}