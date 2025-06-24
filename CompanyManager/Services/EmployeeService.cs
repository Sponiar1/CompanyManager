using CompanyManager.Data;
using CompanyManager.Models;
using CompanyManager.Services.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManager.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly CompanyContext _context;
        public EmployeeService(CompanyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load employees from database");
            }
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            try
            {
                return await _context.Employees.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load employee from database");
            }
        }
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed");
            }
        }
        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            try
            {
                var original = await _context.Employees.AsNoTracking().FirstAsync(e => e.Id_Employee == id);
                if (original == null)
                {
                    return null;
                }
                _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed");
            }
        }
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.Include(e => e.Companies)
                                                   .Include(e => e.Divisions)
                                                   .Include(e => e.Departments)
                                                   .Include(e => e.Projects)
                                                   .FirstOrDefaultAsync(e => e.Id_Employee == id);
            if (employee == null)
            {
                return false;
            }
            if (!employee.Companies.IsNullOrEmpty() ||
               !employee.Divisions.IsNullOrEmpty() ||
               !employee.Departments.IsNullOrEmpty() ||
               !employee.Projects.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Employee is manager of a unit");
            }
            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed");
            }
        }
    }
}
