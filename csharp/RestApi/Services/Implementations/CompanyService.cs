using MongoDB.Driver;
using RestApi.Models;
using RestApi.Repositories;
using RestApi.Repositories.Implementations;

namespace RestApi.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Company> GetCompanyByIdAsync(string id)
    {
        return await _companyRepository.GetCompanyByIdAsync(id);
    }
    public async Task InsertCompanyAsync(Company company)
    {
        await _companyRepository.InsertCompanyAsync(company);
    }
    public async Task<List<Company>> GetCompaniesAsync()
    {
        return await _companyRepository.GetCompaniesAsync();
    }

    public async Task UpdateCompanyAsync(string id, Company company)
    {
        await _companyRepository.UpdateCompanyAsync(id, company);
    }

    public async Task DeleteCompanyAsync(string id)
    {
        await _companyRepository.DeleteCompanyAsync(id);
    }

    public async Task<List<Company>> GetCompaniesByUserAsync(string userId)
    {
        return await _companyRepository.GetCompaniesByUserAsync(userId);
    }
}