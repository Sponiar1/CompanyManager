using CompanyManager.Data;
using CompanyManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Services
{
    public class DivisionService
    {
        private readonly CompanyContext _context;

        public DivisionService(CompanyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Division>> GetAllDivisionsAsync()
        {
            return await _context.Divisions.ToListAsync();
        }
        public async Task<Models.Division> GetDivisionByIdAsync(int id)
        {
            return await _context.Divisions.FindAsync(id);
        }
        public async Task<Models.Division> AddDivisionAsync(Models.Division division)
        {
            var boss = await _context.Employees.FindAsync(division.Id_Boss);
            if (boss == null)
            {
                throw new Exception("Database update failed: Employee (Boss) does not exist.");
            }
            var company = await _context.Companies.FindAsync(division.Id_Company);
            if (company == null)
            {
                throw new Exception("Database update failed: Company does not exist.");
            }
            try
            {
                _context.Divisions.Add(division);
                await _context.SaveChangesAsync();
                return division;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<Models.Division> UpdateDivisionAsync(int id, Models.Division division)
        {
            var original = await _context.Divisions.AsNoTracking().FirstOrDefaultAsync(d => d.Id_Division == id);
            if (original == null)
            {
                return null;
            }
            var boss = await _context.Employees.FindAsync(division.Id_Boss);
            if (boss == null)
            {
                throw new Exception("Database update failed: Employee (Boss) does not exist.");
            }
            var company = await _context.Companies.FindAsync(division.Id_Company);
            if (company == null)
            {
                throw new Exception("Database update failed: Company does not exist.");
            }
            _context.Entry(division).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return division;
        }
        public async Task<bool> DeleteDivisionAsync(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            if (division != null)
            {
                _context.Divisions.Remove(division);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
