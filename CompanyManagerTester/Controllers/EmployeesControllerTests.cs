using CompanyManager.Controllers;
using CompanyManager.Mappers;
using CompanyManager.Models;
using CompanyManager.Services;
using CompanyManager.Services.Templates;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CompanyManagerTester.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly EmployeesController _controller;
        private readonly Mock<IEmployeeService> _employeeServiceMock;

        
        public EmployeesControllerTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeesController(_employeeServiceMock.Object);
        }

        [Fact]
        public async void GetEmployeesSuccess()
        {
            var employees = new List<Employee>
            {
                new Employee
                {
                    First_Name = "John",
                    Last_Name = "Doe",
                    Title = "Mgr",
                    Phone = "123456789",
                    Email = "john.doe@example.com"
                },
                new Employee
                {
                    First_Name = "Jane",
                    Last_Name = "Smith",
                    Title = "Bc.",
                    Phone = "987654321",
                    Email = "jane.smith@example.com"
                }
            };

            _employeeServiceMock.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);
            var result = await _controller.GetEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
            Assert.Contains(returnValue, e => e.First_Name == "John" && e.Last_Name == "Doe");
            Assert.Contains(returnValue, e => e.First_Name == "Jane" && e.Last_Name == "Smith");
        }
        [Fact]
        public async void GetEmployeesNotFound()
        {
            _employeeServiceMock
                .Setup(s => s.GetAllEmployeesAsync())
                .ReturnsAsync(new List<Employee>());
            var result = await _controller.GetEmployees();
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No employees found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetEmployeesException()
        {
            _employeeServiceMock
                .Setup(s => s.GetAllEmployeesAsync())
                .ThrowsAsync(new Exception("Failed to load employee from database"));
            var result = await _controller.GetEmployees();
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Failed to load employee from database", objectResult.Value);
        }
        [Fact]
        public async void GetEmployeeSuccess()
        {
            _employeeServiceMock.Setup(s=>s.GetEmployeeByIdAsync(1))
                .ReturnsAsync(CreateDummy());
            var result = await _controller.GetEmployee(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal("John", returnValue.First_Name);
        }
        [Fact]
        public async void GetEmployeeNotFound()
        {
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);
            var result = await _controller.GetEmployee(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Employee was not found.", notFoundResult.Value);
        }
        [Fact]
        public async void GetEmployeeException()
        {
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1))
                .ThrowsAsync(new Exception("Failed to load employee from database"));
            var result = await _controller.GetEmployee(1);
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Failed to load employee from database", objectResult.Value);
        }
        [Fact]
        public async void PutEmployeeSuccess()
        {
            Employee dummy = CreateDummy();
            EmployeeDTO dummyDTO = CreateDummyDTO();
            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(1, It.IsAny<Employee>())).ReturnsAsync(dummy);
            var result = await _controller.PutEmployee(1, dummyDTO);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal(1, returnValue.Id_Employee);
            Assert.Equal(dummy.First_Name, returnValue.First_Name);
        }
        [Fact]
        public async void PutEmployeeEmpty()
        {
            var result = await _controller.PutEmployee(1, null);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Employee data is required", badRequestResult.Value);
        }
        [Fact]
        public async void PutEmployeeValidationFailure()
        {
            EmployeeDTO dummyDTO = CreateDummyDTO();
            dummyDTO.Email = "invalidEmail";
            var result = await _controller.PutEmployee(1, dummyDTO);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var error = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Equal("Invalid email address.", ((string[])error["Email"])[0]);
        }
        [Fact]
        public async void PutEmployeeNotFound()
        {
            EmployeeDTO employeeDTO = CreateDummyDTO();
            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(1, It.IsAny<Employee>())).ReturnsAsync((Employee)null);
            var result = await _controller.PutEmployee(1,employeeDTO);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee was not found.", notFoundResult.Value);
        }
        [Fact]
        public async void PutEmployeeException()
        {
            EmployeeDTO employeeDTO = CreateDummyDTO();
            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(1, It.IsAny<Employee>()))
                .ThrowsAsync(new Exception("Database update failed"));
            var result = await _controller.PutEmployee(1, employeeDTO);
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Database update failed", objectResult.Value);
        }
        [Fact]
        public async void AddEmployeeSuccess()
        {
            var employee = CreateDummy();
            var employeeDTO = CreateDummyDTO();
            _employeeServiceMock.Setup(s => s.AddEmployeeAsync(It.IsAny<Employee>())).ReturnsAsync(employee);
            var result = await _controller.PostEmployee(employeeDTO);
            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Employee>(createdResult.Value);
            Assert.Equal(employeeDTO.First_Name, returnValue.First_Name);
        }
        [Fact]
        public async void AddEmployeeValidationFailure()
        {
            var employeeDTO = CreateDummyDTO();
            employeeDTO.Email = "invalidEmail";
            var result = await _controller.PostEmployee(employeeDTO);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var error = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.Equal("Invalid email address.", ((string[])error["Email"])[0]);
        }
        [Fact]
        public async void AddEmployeeException()
        {
            var employeeDTO = CreateDummyDTO();
            _employeeServiceMock.Setup(s => s.AddEmployeeAsync(It.IsAny<Employee>()))
                .ThrowsAsync(new Exception("Database update failed"));
            var result = await _controller.PostEmployee(employeeDTO);
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Database update failed", objectResult.Value);

        }
        public async void DeleteEmployeeSuccess()
        {
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(true);
            var result = await _controller.DeleteEmployee(1);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Employee was deleted successfully.", okResult.Value);
        }
        [Fact]
        public async void DeleteEmployeeNotFound()
        {
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(false);
            var result = await _controller.DeleteEmployee(1);
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee was not found.", notFoundResult.Value);
        }
        [Fact]
        public async void DeleteEmployeeException()
        {
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1))
                .ThrowsAsync(new Exception("Database delete failed"));
            var result = await _controller.DeleteEmployee(1);
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Database delete failed", objectResult.Value);
        }
        [Fact]
        public async void DeleteEmployeeInvalid()
        {
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1))
                .ThrowsAsync(new InvalidOperationException("Employee is manager of a unit"));
            var result = await _controller.DeleteEmployee(1);
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(409, objectResult.StatusCode);
            Assert.Equal("Employee is manager of a unit", objectResult.Value);
        }
        private EmployeeDTO CreateDummyDTO()
        {
            return new EmployeeDTO
            {
                First_Name = "John",
                Last_Name = "Doe",
                Title = "Mgr",
                Phone = "123456789",
                Email = "john.doe@example.com"
            };
        }
        private Employee CreateDummy()
        {
            return new Employee
            {
                Id_Employee = 1,
                First_Name = "John",
                Last_Name = "Doe",
                Title = "Mgr",
                Phone = "123456789",
                Email = "john.doe@example.com"
            };
        }
    }
}