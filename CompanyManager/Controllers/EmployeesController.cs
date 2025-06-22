using CompanyManager.Mappers;
using CompanyManager.Mappers.Validator;
using CompanyManager.Models;
using CompanyManager.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CompanyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if(employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return Ok(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                    return NotFound($"Employee with ID {id} not found.");
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (!deleted)
            {
                return NotFound($"Employee with ID {id} not found.");
            }
            return Ok(deleted);
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
