using MongoDB.Driver;
using RestApi.Models;

namespace RestApi.Repositories.Implementations;

public class CompanyRepository : ICompanyRepository
{
    private readonly IMongoCollection<Company> _collection;

    public CompanyRepository(IMongoDatabase database)
    {
        var collectionAttribute = (BsonCollectionAttribute)Attribute.GetCustomAttribute(typeof(Company), typeof(BsonCollectionAttribute));
        var collectionName = collectionAttribute?.CollectionName ?? typeof(Company).Name.ToLower();
        _collection = database.GetCollection<Company>(collectionName);
    }

    async Task<Company> ICompanyRepository.GetCompanyByIdAsync(string id)
    {
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    async Task ICompanyRepository.InsertCompanyAsync(Company company)
    {
        await _collection.InsertOneAsync(company);
    }

    async Task<List<Company>> ICompanyRepository.GetCompaniesAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    async Task ICompanyRepository.UpdateCompanyAsync(string id, Company company)
    {
        await _collection.ReplaceOneAsync(c => c.Id == company.Id, company);
    }

    async Task ICompanyRepository.DeleteCompanyAsync(string id)
    {
        await _collection.DeleteOneAsync(c => c.Id == id);
    }

    async Task<List<Company>> ICompanyRepository.GetCompaniesByUserAsync(string userId)
    {
        return await _collection.Find(c => c.UserId == userId).ToListAsync();
    }
}