using CompanyManager.Data;
using CompanyManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManager.Services
{
    public class CompanyService
    {
        private readonly CompanyContext _context;
        public CompanyService(CompanyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            try
            {
                return await _context.Companies.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load companies from database: " + ex.Message);
            }
        }
        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            try
            {
                return await _context.Companies.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load company from database: " + ex.Message);
            }
        }
        public async Task<Company> AddCompanyAsync(Company company)
        {
            var boss = await _context.Employees.FindAsync(company.Id_Boss);
            if (boss == null)
            {
                throw new ArgumentException("Database update failed: Employee (Boss) does not exist.");
            }
            try
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<Company> UpdateCompanyAsync(int id, Company company)
        {
            var original = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Id_Company == id);
            if (original == null)
            {
                return null;
            }
            var boss = await _context.Employees.FindAsync(company.Id_Boss);
            if (boss == null)
            {
                throw new ArgumentException("Database update failed: Employee (Boss) does not exist.");
            }
            try
            {
                _context.Entry(company).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.Include(c => c.Divisions)
                                                  .FirstOrDefaultAsync(c => c.Id_Company == id);
            if (company == null)
            {
                return false;
            }
            if (!company.Divisions.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Company is associated with division");
            }
            try
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
