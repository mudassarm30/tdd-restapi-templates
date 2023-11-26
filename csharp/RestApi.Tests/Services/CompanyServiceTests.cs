// CompanyServiceTests.cs
using Xunit;
using Moq;
using MongoDB.Driver;
using RestApi.Models;
using RestApi.Services;
using RestApi.Services.Implementations;
using RestApi.Repositories;
using System.Threading.Tasks;

namespace RestApi.Tests.Services
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _mockRepository;
        private readonly CompanyService _service;

        public CompanyServiceTests()
        {
            _mockRepository = new Mock<ICompanyRepository>();
            _service = new CompanyService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ReturnsCompany()
        {
            // Arrange
            var expectedCompany = new Company { Id = "1", Name = "Test Company" };
            _mockRepository.Setup(r => r.GetCompanyByIdAsync("1")).ReturnsAsync(expectedCompany);

            // Act
            var result = await _service.GetCompanyByIdAsync("1");

            // Assert
            Assert.Equal(expectedCompany, result);
        }

        [Fact]
        public async Task GetCompaniesAsync_ReturnsAllCompanies()
        {
            // Arrange
            var expectedCompanies = new List<Company> { new Company { Id = "1", Name = "Test Company" } };
            _mockRepository.Setup(r => r.GetCompaniesAsync()).ReturnsAsync(expectedCompanies);

            // Act
            var result = await _service.GetCompaniesAsync();

            // Assert
            Assert.Equal(expectedCompanies, result);
        }

        [Fact]
        public async Task InsertCompanyAsync_AddsCompany()
        {
            // Arrange
            var newCompany = new Company { Id = "1", Name = "Test Company" };

            // Act
            await _service.InsertCompanyAsync(newCompany);

            // Assert
            _mockRepository.Verify(r => r.InsertCompanyAsync(newCompany), Times.Once);
        }

        [Fact]
        public async Task UpdateCompanyAsync_UpdatesCompany()
        {
            // Arrange
            var existingCompany = new Company { Id = "1", Name = "Test Company" };

            // Act
            await _service.UpdateCompanyAsync("1", existingCompany);

            // Assert
            _mockRepository.Verify(r => r.UpdateCompanyAsync("1", existingCompany), Times.Once);
        }

        [Fact]
        public async Task DeleteCompanyAsync_DeletesCompany()
        {
            // Arrange
            var companyId = "1";

            // Act
            await _service.DeleteCompanyAsync(companyId);

            // Assert
            _mockRepository.Verify(r => r.DeleteCompanyAsync(companyId), Times.Once);
        }

        [Fact]
        public async Task GetCompaniesByUserAsync_ReturnsCompanies()
        {
            // Arrange
            var userId = "1";
            var expectedCompanies = new List<Company> { new Company { Id = "1", Name = "Test Company", UserId = userId } };
            _mockRepository.Setup(r => r.GetCompaniesByUserAsync(userId)).ReturnsAsync(expectedCompanies);

            // Act
            var result = await _service.GetCompaniesByUserAsync(userId);

            // Assert
            Assert.Equal(expectedCompanies, result);
        }
    }
}