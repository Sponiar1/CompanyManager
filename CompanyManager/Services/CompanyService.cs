using CompanyManager.Data;
using CompanyManager.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Companies.ToListAsync();
        }
        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }
        public async Task<Company> AddCompanyAsync(Company company)
        {
            var boss = await _context.Employees.FindAsync(company.Id_Boss);
            if (boss == null)
            {
                throw new Exception("Database update failed: Employee (Boss) does not exist.");
            }
            try
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                return company;
            }
            catch (DbUpdateException ex)
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
                throw new Exception("Database update failed: Employee (Boss) does not exist.");
            }
            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return company;
        }
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
