using CompanyManager.Mappers;
using CompanyManager.Mappers.Validator;
using CompanyManager.Models;
using CompanyManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompaniesController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound($"Company with ID {id} not found.");
            }

            return Ok(company);
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                return Ok(company);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
                return Ok(company);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var deleted = await _companyService.DeleteCompanyAsync(id);
            if (!deleted)
            {
                return NotFound($"Company with ID {id} not found.");
            }
            return Ok(deleted);
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
