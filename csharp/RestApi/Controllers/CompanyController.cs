using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers;

[ApiController]
[Route("Company")]
public class CompanyController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<CompanyController> _logger;

    public CompanyController(ILogger<CompanyController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetCompany")]
    public IActionResult Get()
    {
        return Ok(new Company
        {
            Id = 1,
            Name = "Company Name",
            Address = "Company Address",
            Phone = "Company Phone",
            Email = "Company Email",
            Website = "Company Website",
            Description = "Company Description",
            Logo = "Company Logo"
        });
    }
}
