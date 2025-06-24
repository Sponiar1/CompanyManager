using CompanyManager.Mappers;
using CompanyManager.Mappers.Validator;
using CompanyManager.Models;
using CompanyManager.Services;
using CompanyManager.Services.Templates;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                if (employees == null || !employees.Any())
                {
                    return NotFound("No employees found.");
                }
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound($"Employee was not found.");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromBody] EmployeeDTO updatedEmployee)
        {
            if (updatedEmployee == null)
            {
                return BadRequest("Employee data is required");
            }
            Employee emp = mapEmployee(updatedEmployee,id);
            if (!DataValidator.Validate(emp, out var results))
            {
                foreach (var error in results)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        ModelState.AddModelError(memberName, error.ErrorMessage);
                    }

                    if (!error.MemberNames.Any())
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                return BadRequest(ModelState);
            }

            try
            {
                var employee = await _employeeService.UpdateEmployeeAsync(id, emp);
                if (employee == null)
                {
                    return NotFound("Employee was not found.");
                }
                return Ok(employee);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee([FromBody] EmployeeDTO newEmployee)
        {
            if(newEmployee == null)
            {
                return BadRequest("Employee data is required");
            }    
            Employee emp = mapEmployee(newEmployee);
            if (!DataValidator.Validate(emp, out var results))
            {
                foreach (var error in results)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        ModelState.AddModelError(memberName, error.ErrorMessage);
                    }

                    if (!error.MemberNames.Any())
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }

                return BadRequest(ModelState);
            }

            try
            {
                var employee = await _employeeService.AddEmployeeAsync(emp);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id_Employee }, employee);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var deleted = await _employeeService.DeleteEmployeeAsync(id);
                if (!deleted)
                {
                    return NotFound($"Employee was not found.");
                }
                return Ok(deleted);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private Employee mapEmployee(EmployeeDTO employeeDTO, int? id = null)
        {
            Employee employee = new Employee
            {
                Id_Employee = id ?? 0,
                First_Name = employeeDTO.First_Name,
                Last_Name = employeeDTO.Last_Name,
                Title = employeeDTO.Title,
                Phone = employeeDTO.Phone,
                Email = employeeDTO.Email,
            };
            return employee;
        }
    }
}
