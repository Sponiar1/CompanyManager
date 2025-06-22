using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyManager.Data;
using CompanyManager.Models;
using CompanyManager.Services;
using CompanyManager.Mappers;
using CompanyManager.Mappers.Validator;

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
            return await _departmentService.GetAllDepartmentsAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound($"Department with ID {id} not found.");
            }
            return Ok(department);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, [FromBody]DepartmentDTO updatedDepartment)
        {
            if(updatedDepartment == null)
            {
                return BadRequest("Department data is required");
            }
            Department dep = await _departmentService.UpdateDepartmentAsync(id, updatedDepartment);
            if(!DataValidator.Validate(dep, out var results))
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
                if (updated == null)
                {
                    return NotFound($"Department with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
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
            Department dep = await _departmentService.AddDepartmentAsync(newDepartment);
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
                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);
                if (!deleted)
                {
                    return NotFound($"Department with ID {id} not found.");
                }
                return Ok(deleted);
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
