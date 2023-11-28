using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RestApi.Models;
using RestApi.Repositories;

namespace RestApi.Services.Implementations;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly JwtService _jwtService;

    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, JwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _userRepository.GetUserByUsername(username);
    }

    public async Task CreateUser(User user)
    {
        await _userRepository.CreateUser(user);
    }

    public async Task UpdateUser(User user)
    {
        await _userRepository.UpdateUser(user);
    }
    public async Task<string> Login(string username, string password)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return null;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return _jwtService.GenerateToken(user);
    }
}