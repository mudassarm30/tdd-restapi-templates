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
    }
}