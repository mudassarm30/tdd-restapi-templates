using RestApi.Models;

namespace RestApi.Services;
public interface IUserService
{
    Task<User> GetUserByUsername(string username);
    Task CreateUser(User user);
    Task UpdateUser(User user);
    Task<string> Login(string username, string password);
}