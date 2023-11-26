// CompanyControllerTests.cs
using Xunit;
using Moq;
using RestApi.Models;
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
        private readonly CompanyController _controller;

        public CompanyControllerTests()
        {
            _mockService = new Mock<ICompanyService>();
            var mockLogger = new Mock<ILogger<CompanyController>>();
            _controller = new CompanyController(mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task Get_ReturnsCompany()
        {
            
            // Arrange
            var expectedCompany = new Company { Id = "1", Name = "Test Company" };
            _mockService.Setup(service => service.GetCompanyByIdAsync("1"))
                .ReturnsAsync(expectedCompany);

            // Act
            var result = await _controller.Retrieve("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Company>(okResult.Value);
            Assert.Equal(expectedCompany, returnValue);
            
        }
    }
}