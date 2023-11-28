using RestApi.Models;

namespace RestApi.Repositories;
public interface IUserRepository
{
    Task<User> GetUserByUsername(string username);
    Task CreateUser(User user);
    Task UpdateUser(User user);
}