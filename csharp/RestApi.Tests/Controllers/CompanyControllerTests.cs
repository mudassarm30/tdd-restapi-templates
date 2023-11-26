// CompanyControllerTests.cs
using Xunit;
using Moq;
using RestApi.Models;
using RestApi.Models.Payloads;
using RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RestApi.Controllers;
using Microsoft.Extensions.Logging;

namespace RestApi.Tests.Controllers
{
    public class CompanyControllerTests
    {
        private readonly Mock<ICompanyService> _mockService;
        private readonly ILogger<CompanyController> _logger;
        private readonly CompanyController _controller;

        public CompanyControllerTests()
        {
            _mockService = new Mock<ICompanyService>();
            _controller = new CompanyController(_logger, _mockService.Object);
        }

        [Fact]
        public async Task GetCompanyByIdAsync_ReturnsCompany()
        {
            // Arrange
            var expectedCompany = new Company { Id = "1", Name = "Test Company" };
            _mockService.Setup(s => s.GetCompanyByIdAsync("1")).ReturnsAsync(expectedCompany);

            // Act
            var actionResult = await _controller.GetCompanyByIdAsync("1");
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            var result = objectResult.Value as Company;
            Assert.Equal(expectedCompany, result);
        }

        [Fact]
        public async Task GetCompaniesAsync_ReturnsAllCompanies()
        {
            // Arrange
            var expectedCompanies = new List<Company> { new Company { Id = "1", Name = "Test Company" } };
            _mockService.Setup(s => s.GetCompaniesAsync()).ReturnsAsync(expectedCompanies);

            // Act
            var actionResult = await _controller.GetCompaniesAsync();
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            var result = objectResult.Value as List<Company>;
            Assert.Equal(expectedCompanies, result);
        }

        [Fact]
        public async Task InsertCompanyAsync_AddsCompany()
        {
            // Arrange
            var newCompany = new Company { Name = "Test Company" };
            var createCompanyRequest = new CreateCompanyRequest { Name = newCompany.Name };
            _mockService.Setup(s => s.InsertCompanyAsync(It.IsAny<Company>()));

            // Act
            var actionResult = await _controller.InsertCompanyAsync(createCompanyRequest);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            var result = objectResult.Value as Company;
            Assert.Equal(newCompany.Name, result.Name);
        }

        [Fact]
        public async Task UpdateCompanyAsync_UpdatesCompany()
        {
            var existingCompanyRequest = new UpdateCompanyRequest { Name = "Test Company" };
            var existingCompany = new Company { Id = "1", Name = "Test Company" };

            _mockService.Setup(s => s.GetCompanyByIdAsync("1")).ReturnsAsync(existingCompany);

            // Act
            await _controller.UpdateCompanyAsync("1", existingCompanyRequest);

            // Assert
            _mockService.Verify(s => s.UpdateCompanyAsync("1", It.IsAny<Company>()), Times.Once);
        }


        [Fact]
        public async Task DeleteCompanyAsync_DeletesCompany()
        {
            // Arrange
            var companyId = "1";
            var existingCompany = new Company { Id = "1", Name = "Test Company" };

            _mockService.Setup(s => s.GetCompanyByIdAsync("1")).ReturnsAsync(existingCompany);

            // Act
            await _controller.DeleteCompanyAsync(companyId);

            // Assert
            _mockService.Verify(s => s.DeleteCompanyAsync(companyId), Times.Once);
        }


        [Fact]
        public async Task GetCompaniesByUserAsync_ReturnsCompanies()
        {
            // Arrange
            var userId = "1";
            var expectedCompanies = new List<Company> { new Company { Id = "1", Name = "Test Company", UserId = userId } };
            _mockService.Setup(s => s.GetCompaniesByUserAsync(userId)).ReturnsAsync(expectedCompanies);

            // Act
            var actionResult = await _controller.GetCompaniesByUserAsync(userId);
            var objectResult = actionResult as ObjectResult;

            // Assert
            Assert.NotNull(objectResult);
            var result = objectResult.Value as List<Company>;
            Assert.Equal(expectedCompanies, result);
        }

    }
}