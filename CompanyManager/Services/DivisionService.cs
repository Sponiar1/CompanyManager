using CompanyManager.Data;
using CompanyManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManager.Services
{
    public class DivisionService
    {
        private readonly CompanyContext _context;

        public DivisionService(CompanyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Division>> GetAllDivisionsAsync()
        {
            return await _context.Divisions.ToListAsync();
        }
        public async Task<Division> GetDivisionByIdAsync(int id)
        {
            return await _context.Divisions.FindAsync(id);
        }
        public async Task<Division> AddDivisionAsync(Division division)
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
        public async Task<Division> UpdateDivisionAsync(int id, Division division)
        {
            var original = await _context.Divisions.AsNoTracking().FirstOrDefaultAsync(d => d.Id_Division == id);
            if (original == null)
            {
                throw new Exception("Database update failed: Division does not exist.");
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
            try
            {
                _context.Entry(division).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return division;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<bool> DeleteDivisionAsync(int id)
        {
            var division = await _context.Divisions.Include(c => c.Projects)
                                              .FirstOrDefaultAsync(c => c.Id_Company == id);
            if (division == null)
            {
                return false;
            }
            if (!division.Projects.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Division cannot be deleted because it has associated projects.");
            }
            _context.Divisions.Remove(division);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
