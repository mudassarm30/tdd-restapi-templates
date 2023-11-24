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
            Date = DateTime.Now.AddDays(1),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        });
    }
}
