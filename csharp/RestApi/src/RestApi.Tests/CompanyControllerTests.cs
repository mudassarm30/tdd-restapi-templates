using Xunit;
using RestApi.Controllers;
using RestApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RestApi.Tests
{
    public class CompanyControllerTests
    {
        [Fact]
        public void Test_GetCompanies()
        {
            var options = new DbContextOptionsBuilder<CompanyContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetCompanies")
                .Options;

            using (var context = new CompanyContext(options))
            {
                var controller = new CompanyController(context);

                context.Companies.Add(new Company { Name = "Test Company", Address = "Test Address" });
                context.SaveChanges();

                var result = controller.GetCompanies();

                Assert.Equal(1, result.Value.Count());
                Assert.Equal("Test Company", result.Value.First().Name);
                Assert.Equal("Test Address", result.Value.First().Address);
            }
        }

        // Add more tests for POST, PUT, DELETE methods
    }
}