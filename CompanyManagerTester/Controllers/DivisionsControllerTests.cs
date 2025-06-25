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
    public class DivisionsControllerTests
    {
        private readonly DivisionsController _controller;
        private readonly Mock<IDivisionService> _divisionServiceMock;

        public DivisionsControllerTests()
        {
            _divisionServiceMock = new Mock<IDivisionService>();
            _controller = new DivisionsController(_divisionServiceMock.Object);
        }
        [Fact]
        public async void GetDivisionsSuccess()
        {
            var divisions = new List<Division>
            {
                new Division { Id_Division = 1, Div_Name = "Test Division 1", Code = "D01", Id_Company = 1 },
                new Division { Id_Division = 2, Div_Name = "Test Division 2", Code = "D02", Id_Company = 2 }
            };
            _divisionServiceMock.Setup(s => s.GetAllDivisionsAsync()).ReturnsAsync(divisions);
            var result = await _controller.GetDivisions();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Division>>(okResult.Value);
            Assert.NotNull(result);
            Assert.Contains(returnValue, d => d.Div_Name == "Test Division 1" && d.Code == "D01");
            Assert.Contains(returnValue, d => d.Div_Name == "Test Division 2" && d.Code == "D02");
        }
        [Fact]
        public async void GetDivisionsNotFound()
        {
            _divisionServiceMock.Setup(s => s.GetAllDivisionsAsync()).ReturnsAsync(new List<Division>());
            var result = await _controller.GetDivisions();
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No divisions found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetDivisionsException()
        {
            _divisionServiceMock.Setup(s => s.GetAllDivisionsAsync()).Throws(new Exception("Database error"));
            var result = await _controller.GetDivisions();
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Database error", statusCodeResult.Value);
        }
        [Fact]
        public async void GetDivisionByIdSuccess()
        {
            var division = CreateDivision();
            _divisionServiceMock.Setup(s => s.GetDivisionByIdAsync(1)).ReturnsAsync(division);
            var result = await _controller.GetDivision(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Division>(okResult.Value);
            Assert.Equal("Test Division", returnValue.Div_Name);
        }
        [Fact]
        public async void GetDivisionByIdNotFound()
        {
            _divisionServiceMock.Setup(s => s.GetDivisionByIdAsync(1)).ReturnsAsync((Division)null);
            var result = await _controller.GetDivision(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Division was not found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetDivisionByIdException()
        {
            _divisionServiceMock.Setup(s => s.GetDivisionByIdAsync(1)).Throws(new Exception("Database error"));
            var result = await _controller.GetDivision(1);
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Database error", statusCodeResult.Value);
        }
        [Fact]
        public async void CreateDivisionSuccess()
        {
            var divisionDTO = CreateDivisionDTO();
            var division = CreateDivision();
            _divisionServiceMock.Setup(s => s.AddDivisionAsync(It.IsAny<Division>())).ReturnsAsync(division);
            var result = await _controller.PostDivision(divisionDTO);
            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Division>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal("Test Division", returnValue.Div_Name);
        }
        [Fact]
        public async void CreateDivisionBossFailure()
        {
            var divisionDTO = CreateDivisionDTO();
            _divisionServiceMock.Setup(s => s.AddDivisionAsync(It.IsAny<Division>())).Throws(new ArgumentException("Database update failed: Employee (Boss) does not exist."));
            var result = await _controller.PostDivision(divisionDTO);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Database update failed: Employee (Boss) does not exist.", badRequestResult.Value);
        }
        [Fact]
        public async void CreateDivisionException()
        {
            var divisionDTO = CreateDivisionDTO();
            _divisionServiceMock.Setup(s => s.AddDivisionAsync(It.IsAny<Division>())).Throws(new Exception("Database error"));
            var result = await _controller.PostDivision(divisionDTO);
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void UpdateDivisionSuccess()
        {
            var divisionDTO = CreateDivisionDTO();
            var division = CreateDivision();
            _divisionServiceMock.Setup(s => s.UpdateDivisionAsync(1,It.IsAny<Division>())).ReturnsAsync(division);
            var result = await _controller.PutDivision(1, divisionDTO);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Division>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal("Test Division", returnValue.Div_Name);
        }
        [Fact]
        public async void DeleteDivisionSuccess()
        {
            _divisionServiceMock.Setup(s => s.DeleteDivisionAsync(1)).ReturnsAsync(true);
            var result = await _controller.DeleteDivision(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }
        [Fact]
        public async void DeleteDivisionNotFound()
        {
            _divisionServiceMock.Setup(s => s.DeleteDivisionAsync(1)).ReturnsAsync(false);
            var result = await _controller.DeleteDivision(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Division was not found.", notFoundResult.Value);
        }

        private Division CreateDivision()
        {
            return new Division
            {
                Id_Division = 1,
                Div_Name = "Test Division",
                Code = "D01",
                Id_Company = 1,
                Id_Boss = 1
            };
        }
        private DivisionDTO CreateDivisionDTO()
        {
            return new DivisionDTO
            {
                Div_Name = "Test Division",
                Code = "D01",
                Id_Company = 1,
                Id_Boss = 1
            };
        }
    }
}
