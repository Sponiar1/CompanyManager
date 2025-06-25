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
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            try
            {
                var company = await _companyService.GetAllCompaniesAsync();
                if (company == null || !company.Any())
                {
                    return NotFound("No companies found.");
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message );
            }
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);
                if (company == null)
                {
                    return NotFound($"Company was not found.");
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, [FromBody] CompanyDTO updatedCompany)
        {
            if (updatedCompany == null)
            {
                return BadRequest("Company data is required");
            }
            Company com = mapCompany(updatedCompany, id);
            if (!DataValidator.Validate(com, out var results))
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
                var company = await _companyService.UpdateCompanyAsync(id,com);
                if(company == null)
                {
                    return NotFound($"Company does not exists.");
                }
                return Ok(company);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany([FromBody] CompanyDTO newCompany)
        {
            if (newCompany == null)
            {
                return BadRequest("Company data is required");
            }
           Company com = mapCompany(newCompany);
            if (!DataValidator.Validate(com, out var results))
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
                var company = await _companyService.AddCompanyAsync(com);
                return CreatedAtAction(nameof(GetCompany), new { id = company.Id_Company }, company);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message );
            }
        }
        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid company ID.");
            }
            try {
                var deleted = await _companyService.DeleteCompanyAsync(id);
                if (!deleted)
                {
                    return NotFound($"Company does not exists.");
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

        private Company mapCompany(CompanyDTO company, int? id = null)
        {
            return new Company
            {
                Id_Company = id ?? 0,
                Com_Name = company.Com_Name,
                Code = company.Code,
                Id_Boss = company.Id_Boss
            };
        }
    }
}
