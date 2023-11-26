using RestApi.Models;

namespace RestApi.Services;

public interface ICompanyService
{
    Task<Company> GetCompanyByIdAsync(string id);
    Task InsertCompanyAsync(Company company);
    Task<List<Company>> GetCompaniesAsync();
    Task UpdateCompanyAsync(string id, Company company);
    Task DeleteCompanyAsync(string id);
    Task<List<Company>> GetCompaniesByUserAsync(string userId);

}