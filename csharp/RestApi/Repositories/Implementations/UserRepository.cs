using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestApi.Models;

namespace RestApi.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(IMongoClient client, IOptions<MongoDBConfig> settings, IPasswordHasher<User> passwordHasher)
    {
        _users = client.GetDatabase(settings.Value.DatabaseName).GetCollection<User>(DatabaseInitializer.GetCollectionName<User>());
        _passwordHasher = passwordHasher;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
    }

    public async Task CreateUser(User user)
    {
        user.Password = _passwordHasher.HashPassword(user, user.Password);
        await _users.InsertOneAsync(user);
    }

    public async Task UpdateUser(User user)
    {
        user.Password = _passwordHasher.HashPassword(user, user.Password);
        await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
    }
}