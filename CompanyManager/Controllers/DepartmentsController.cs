using CompanyManager.Mappers;
using CompanyManager.Mappers.Validator;
using CompanyManager.Models;
using CompanyManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();
                if (departments == null || !departments.Any())
                {
                    return NotFound("No departments found.");
                }
                return Ok(departments);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentByIdAsync(id);
                if (department == null)
                {
                    return NotFound($"Department with ID {id} not found.");
                }
                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, [FromBody] DepartmentDTO updatedDepartment)
        {
            if (updatedDepartment == null)
            {
                return BadRequest("Department data is required");
            }
            Department dep = mapDepartment(updatedDepartment, id);
            if (!DataValidator.Validate(dep, out var results))
            {
                foreach (var error in results)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        ModelState.AddModelError(memberName, error.ErrorMessage);
                    }
                    if (!error.MemberNames.Any())
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
                return BadRequest(ModelState);
            }
            try
            {
                var updated = await _departmentService.UpdateDepartmentAsync(id, dep);
                if(updated == null)
                {
                    return NotFound($"Department was not found.");
                }
                return Ok(updated);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment([FromBody] DepartmentDTO newDepartment)
        {
            if (newDepartment == null)
            {
                return BadRequest("Department data is required");
            }
            Department dep = mapDepartment(newDepartment);
            if (!DataValidator.Validate(dep, out var results))
            {
                foreach (var error in results)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        ModelState.AddModelError(memberName, error.ErrorMessage);
                    }
                    if (!error.MemberNames.Any())
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
                return BadRequest(ModelState);
            }
            try
            {
                var department = await _departmentService.AddDepartmentAsync(dep);
                return CreatedAtAction(nameof(GetDepartment), new { id = department.Id_Department }, department);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);
                if (!deleted)
                {
                    return NotFound($"Department was not found.");
                }
                return Ok(deleted);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private Department mapDepartment(DepartmentDTO departmentDTO, int? id = null)
        {
            return new Department
            {
                Id_Department = id ?? 0,
                Dep_Name = departmentDTO.Dep_Name,
                Code = departmentDTO.Code,
                Id_Project = departmentDTO.Id_Project,
                Id_Boss = departmentDTO.Id_Boss
            };
        }
    }
}
