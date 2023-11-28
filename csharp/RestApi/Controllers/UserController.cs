using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using RestApi.Models.Payloads;
using RestApi.Services;
using RestApi.Services.Implementations;

namespace RestApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser(CreateUserRequest createRequest)
    {
        var user = new User
        {
            Username = createRequest.Username,
            Password = createRequest.Password,
            Name = createRequest.Name,
            Address = createRequest.Address,
            City = createRequest.City,
            State = createRequest.State,
            Country = createRequest.Country,
            PostalCode = createRequest.PostalCode,
            Phone = createRequest.Phone,
            Email = createRequest.Email
        };

        await _userService.CreateUser(user);
        return Ok();
    }

    [Authorize]
    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateUser(string username, UpdateUserRequest updateRequest)
    {

        var user = await _userService.GetUserByUsername(username);

        if (user == null)
        {
            return NotFound();
        }

        if (updateRequest.Password != null)
        {
            user.Password = updateRequest.Password;
        }

        if (updateRequest.Name != null)
        {
            user.Name = updateRequest.Name;
        }

        if (updateRequest.Address != null)
        {
            user.Address = updateRequest.Address;
        }

        if (updateRequest.City != null)
        {
            user.City = updateRequest.City;
        }

        if (updateRequest.State != null)
        {
            user.State = updateRequest.State;
        }

        if (updateRequest.Country != null)
        {
            user.Country = updateRequest.Country;
        }

        if (updateRequest.PostalCode != null)
        {
            user.PostalCode = updateRequest.PostalCode;
        }

        if (updateRequest.Phone != null)
        {
            user.Phone = updateRequest.Phone;
        }

        if (updateRequest.Email != null)
        {
            user.Email = updateRequest.Email;
        }

        await _userService.UpdateUser(user);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
        var token = await _userService.Login(user.Username, user.Password);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }
}