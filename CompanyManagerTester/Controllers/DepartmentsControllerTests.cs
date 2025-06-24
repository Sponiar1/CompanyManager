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
    public class DepartmentsControllerTests
    {
        private readonly DepartmentsController _controller;
        private readonly Mock<IDepartmentService> _departmentServiceMock;

        public DepartmentsControllerTests()
        {
            _departmentServiceMock = new Mock<IDepartmentService>();
            _controller = new DepartmentsController(_departmentServiceMock.Object);
        }

        [Fact]
        public async void GetDepartmentsSuccess()
        {
            var departments = new List<Department>
            {
                new Department { Id_Department = 1, Dep_Name = "HR", Id_Project = 1, Id_Boss = 1, Code = "D01" },
                new Department { Id_Department = 2, Dep_Name = "IT", Id_Project = 1, Id_Boss = 2, Code = "D02" }
            };
            _departmentServiceMock.Setup(s => s.GetAllDepartmentsAsync()).ReturnsAsync(departments);
            var result = await _controller.GetDepartments();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Department>>(okResult.Value);
            Assert.NotNull(result);
            Assert.Contains(returnValue, d => d.Dep_Name == "HR" && d.Id_Project == 1);
            Assert.Contains(returnValue, d => d.Dep_Name == "IT" && d.Id_Project == 1);
        }
        [Fact]
        public async void GetDepartmentsNotFound()
        {
            _departmentServiceMock.Setup(s => s.GetAllDepartmentsAsync()).ReturnsAsync(new List<Department>());
            var result = await _controller.GetDepartments();
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No departments found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetDepartmentByIdSuccess()
        {
            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).ReturnsAsync(CreateDepartment());
            var result = await _controller.GetDepartment(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Department>(okResult.Value);
        }
        [Fact]
        public async void GetDepartmentByIdException()
        {
            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).Throws(new Exception("Department was not found."));
            var result = await _controller.GetDepartment(1);
            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal("Department was not found.", notFoundResult.Value);
        }
        [Fact]
        public async void CreateDepartmentSuccess()
        {
            var departmentDTO = CreateDepartmentDTO();
            var department = CreateDepartment();
            _departmentServiceMock.Setup(s => s.AddDepartmentAsync(It.IsAny<Department>())).ReturnsAsync(department);
            var result = await _controller.PostDepartment(departmentDTO);
            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Department>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal(department.Id_Department, returnValue.Id_Department);
            Assert.Equal(department.Dep_Name, returnValue.Dep_Name);
        }
        [Fact]
        public async void CreateDepartmentException()
        {
            var departmentDTO = CreateDepartmentDTO();
            _departmentServiceMock.Setup(s => s.AddDepartmentAsync(It.IsAny<Department>())).Throws(new Exception("Database error"));
            var result = await _controller.PostDepartment(departmentDTO);
            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
        [Fact]
        public async void UpdateDepartmentSuccess()
        {
            var departmentDTO = CreateDepartmentDTO();
            var department = CreateDepartment();
            _departmentServiceMock.Setup(s => s.UpdateDepartmentAsync(1,It.IsAny<Department>())).ReturnsAsync(department);
            var result = await _controller.PutDepartment(1, departmentDTO);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Department>(okResult.Value);
            Assert.NotNull(result);
            Assert.Equal(department.Id_Department, returnValue.Id_Department);
            Assert.Equal(department.Dep_Name, returnValue.Dep_Name);
        }
        [Fact]
        public async void UpdateDepartmentCodeValidationFailure()
        {
            var departmentDTO = CreateDepartmentDTO();
            departmentDTO.Code = "";
            var result = await _controller.PutDepartment(1, departmentDTO);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var error = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Equal("Department code is required.", ((string[])error["Code"])[0]);
        }
        [Fact]
        public async void DeleteDepartmentSuccess()
        {
            _departmentServiceMock.Setup(s => s.DeleteDepartmentAsync(1)).ReturnsAsync(true);
            var result = await _controller.DeleteDepartment(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(true, okResult.Value);
        }
        [Fact]
        public async void DeleteDepartmentNotFound()
        {
            _departmentServiceMock.Setup(s => s.DeleteDepartmentAsync(1)).ReturnsAsync(false);
            var result = await _controller.DeleteDepartment(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Department was not found.", notFoundResult.Value);
        }

        private Department CreateDepartment()
        {
            return new Department
            {
                Id_Department = 1,
                Dep_Name = "Test Department",
                Code = "D01",
                Id_Project = 1,
                Id_Boss = 1,
            };
        }
        private DepartmentDTO CreateDepartmentDTO()
        {
            return new DepartmentDTO
            {
                Dep_Name = "Test Department",
                Code = "D01",
                Id_Project = 1,
                Id_Boss = 1,
            };
        }
    }
}
