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
    public class DivisionsController : ControllerBase
    {
        private readonly DivisionService _divisionService;

        public DivisionsController(DivisionService divisionService)
        {
            _divisionService = divisionService;
        }

        // GET: api/Divisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Division>>> GetDivisions()
        {
            try
            {
                var divisions = await _divisionService.GetAllDivisionsAsync();
                if (divisions == null || !divisions.Any())
                {
                    return NotFound("No divisions found.");
                }
                return Ok(divisions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Divisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Division>> GetDivision(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid division ID.");
            }
            try
            {
                var division = await _divisionService.GetDivisionByIdAsync(id);
                if (division == null)
                {
                    return NotFound($"Division was not found.");
                }
                return Ok(division);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Divisions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivision(int id, [FromBody] DivisionDTO updatedDivision)
        {
           if (updatedDivision == null) 
            {
                return BadRequest("Division data is required");
            }
            Division div = mapDivision(updatedDivision, id);
            if (!DataValidator.Validate(div, out var results))
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
                var division = await _divisionService.UpdateDivisionAsync(id, div);
                if (division == null)
                {
                    return NotFound($"Division was not found.");
                }
                return Ok(division);
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

        // POST: api/Divisions
        [HttpPost]
        public async Task<ActionResult<Division>> PostDivision([FromBody] DivisionDTO newDivision)
        {
            if(newDivision == null)
            {
                return BadRequest("Division data is required");
            }
            Division div = mapDivision(newDivision);
            if (!DataValidator.Validate(div, out var results))
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
                var division = await _divisionService.AddDivisionAsync(div);
                return CreatedAtAction(nameof(GetDivision), new { id = division.Id_Division }, division);
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

        // DELETE: api/Divisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDivision(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid division ID.");
            }
            try
            {
                var deleted = await _divisionService.DeleteDivisionAsync(id);
                if (!deleted)
                {
                    return NotFound($"Division was not found.");
                }
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private Division mapDivision(DivisionDTO divisionDTO, int? id = null)
        {
            return new Division
            {
                Id_Division = id ?? 0,
                Div_Name = divisionDTO.Div_Name,
                Code = divisionDTO.Code,
                Id_Company = divisionDTO.Id_Company,
                Id_Boss = divisionDTO.Id_Boss
            };
        }
    }
}
