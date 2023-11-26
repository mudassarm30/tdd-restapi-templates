using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using RestApi.Models.Payloads;
using RestApi.Services;

namespace RestApi.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
    private readonly ILogger<CompanyController> _logger;
    private readonly ICompanyService _companyService;

    public CompanyController(ILogger<CompanyController> logger, ICompanyService companyService)
    {
        _logger = logger;
        _companyService = companyService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Retrieve(string id)
    {
        try
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting company");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest payload)
    {
        try
        {
            var company = new Company
            {
                Name = payload.Name,
                Address = payload.Address,
                Phone = payload.Phone,
                Email = payload.Email,
                Website = payload.Web,
                Description = payload.Description,
                Logo = payload.Logo
            };

            company.Id = Guid.NewGuid().ToString();
            await _companyService.InsertCompanyAsync(company);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting company");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("")]
    public async Task<IActionResult> List()
    {
        try
        {
            var companies = await _companyService.GetCompaniesAsync();
            if (companies == null)
            {
                return NotFound();
            }

            return Ok(companies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting companies");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            await _companyService.DeleteCompanyAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting company");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCompanyRequest payload)
    {
        try
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            if (payload.Name != null)
            {
                company.Name = payload.Name;
            }

            if (payload.Address != null)
            {
                company.Address = payload.Address;
            }

            if (payload.Phone != null)
            {
                company.Phone = payload.Phone;
            }

            if (payload.Email != null)
            {
                company.Email = payload.Email;
            }

            if (payload.Web != null)
            {
                company.Website = payload.Web;
            }

            if (payload.Description != null)
            {
                company.Description = payload.Description;
            }

            if (payload.Logo != null)
            {
                company.Logo = payload.Logo;
            }

            await _companyService.UpdateCompanyAsync(id, company);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating company");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        try
        {
            var companies = await _companyService.GetCompaniesByUserAsync(userId);
            if (companies == null)
            {
                return NotFound();
            }

            return Ok(companies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting companies by user id");
            return StatusCode(500, "Internal server error");
        }
    }
}
