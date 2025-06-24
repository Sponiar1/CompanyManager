using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyManager.Controllers;
using CompanyManager.Mappers;
using CompanyManager.Models;
using CompanyManager.Services.Templates;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CompanyManagerTester.Controllers
{
    public class CompaniesControllerTests
    {
        private readonly CompaniesController _controller;
        private readonly Mock<ICompanyService> _companyServiceMock;

        public CompaniesControllerTests()
        {
            _companyServiceMock = new Mock<ICompanyService>();
            _controller = new CompaniesController(_companyServiceMock.Object);
        }
        [Fact]
        public async void GetCompaniesSuccess()
        {
            var companies = new List<Company>
            {
                new Company { Id_Company = 1, Com_Name = "Test Company 1", Code = "C01", Id_Boss = 1 },
                new Company { Id_Company = 2, Com_Name = "Test Company 2", Code = "C02", Id_Boss = 2 }
            };

            _companyServiceMock.Setup(s => s.GetAllCompaniesAsync()).ReturnsAsync(companies);
            var result = await _controller.GetCompanies();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Company>>(okResult.Value);
            Assert.NotNull(result);
            Assert.Contains(returnValue, c => c.Com_Name == "Test Company 1" && c.Code == "C01");
            Assert.Contains(returnValue, c => c.Com_Name == "Test Company 2" && c.Code == "C02");
        }
        [Fact]
        public async void GetCompaniesNotFound()
        {
            _companyServiceMock.Setup(s => s.GetAllCompaniesAsync()).ReturnsAsync(new List<Company>());
            var result = await _controller.GetCompanies();
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No companies found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetCompaniesException()
        {
            _companyServiceMock.Setup(s => s.GetAllCompaniesAsync()).Throws(new Exception("Database error"));
            var result = await _controller.GetCompanies();
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void GetCompanySuccess()
        {
            var company = CreateTestCompany();
            _companyServiceMock.Setup(s => s.GetCompanyByIdAsync(1)).ReturnsAsync(company);
            var result = await _controller.GetCompany(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Company>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal("Test Company", returnValue.Com_Name);
        }
        [Fact]
        public async void GetCompanyNotFound()
        {
            _companyServiceMock.Setup(s => s.GetCompanyByIdAsync(1)).ReturnsAsync((Company)null);
            var result = await _controller.GetCompany(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Company was not found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetCompanyException()
        {
            _companyServiceMock.Setup(s => s.GetCompanyByIdAsync(1)).Throws(new Exception("Database error"));
            var result = await _controller.GetCompany(1);
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void PutCompanySuccess()
        {
            var company = CreateTestCompany();
            _companyServiceMock.Setup(s => s.UpdateCompanyAsync(1, It.IsAny<Company>())).ReturnsAsync(company);
            var result = await _controller.PutCompany(1, CreateTestCompanyDTO());
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Company>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal("Test Company", returnValue.Com_Name);
        }
        [Fact]
        public async void PutCompanyNotFound()
        {
            _companyServiceMock.Setup(s => s.UpdateCompanyAsync(1, It.IsAny<Company>())).ReturnsAsync((Company)null);
            var result = await _controller.PutCompany(1, CreateTestCompanyDTO());
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Company does not exists.", notFoundResult.Value);
        }
        [Fact]
        public async void PutCompanyException()
        {
            _companyServiceMock.Setup(s => s.UpdateCompanyAsync(1, It.IsAny<Company>())).Throws(new Exception("Database error"));
            var result = await _controller.PutCompany(1, CreateTestCompanyDTO());
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Database error", objectResult.Value);
        }
        [Fact]
        public async void PutCompanyValidationFailure()
        {
            var invalidCompanyDTO = CreateTestCompanyDTO();
            invalidCompanyDTO.Com_Name = "";
            var result = await _controller.PutCompany(1, invalidCompanyDTO);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var error = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Equal("Company name is required.", ((string[])error["Com_Name"])[0]);
        }
        [Fact]
        public async void PostCompanySuccess()
        {
            var company = CreateTestCompany();
            _companyServiceMock.Setup(s => s.AddCompanyAsync(It.IsAny<Company>())).ReturnsAsync(company);
            var result = await _controller.PostCompany(CreateTestCompanyDTO());
            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Company>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal("Test Company", returnValue.Com_Name);
        }
        [Fact]
        public async void PostCompanyBossException()
        {
            _companyServiceMock.Setup(s => s.AddCompanyAsync(It.IsAny<Company>())).Throws(new ArgumentException("Database update failed: Employee (Boss) does not exist."));
            var result = await _controller.PostCompany(CreateTestCompanyDTO());
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Database update failed: Employee (Boss) does not exist.", badRequestResult.Value);
        }
        [Fact]
        public async void DeleteCompanySuccess()
        {
            var company = CreateTestCompany();
            _companyServiceMock.Setup(s => s.DeleteCompanyAsync(1)).ReturnsAsync(true);
            var result = await _controller.DeleteCompany(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }
        [Fact]
        public async void DeleteCompanyNotFound()
        {
            _companyServiceMock.Setup(s => s.DeleteCompanyAsync(1)).ReturnsAsync(false);
            var result = await _controller.DeleteCompany(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Company does not exists.", notFoundResult.Value);
        }
        [Fact]
        public async void DeleteCompanyInvalid()
        {
            _companyServiceMock.Setup(s => s.DeleteCompanyAsync(1)).Throws(new InvalidOperationException("Company is associated with division"));
            var result = await _controller.DeleteCompany(1);
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Company is associated with division", statusCodeResult.Value);
        }
        private Company CreateTestCompany()
        {
            return new Company
            {
                Id_Company = 1,
                Com_Name = "Test Company",
                Code = "C01",
                Id_Boss = 1
            };
        }
        private CompanyDTO CreateTestCompanyDTO()
        {
            return new CompanyDTO
            {
                Com_Name = "Test Company",
                Code = "C01",
                Id_Boss = 1
            };
        }
    }
}
