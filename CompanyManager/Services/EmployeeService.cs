using CompanyManager.Data;
using CompanyManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManager.Services
{
    public class EmployeeService
    {
        private readonly CompanyContext _context;
        public EmployeeService(CompanyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
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
                throw new InvalidOperationException("Employee is manager of a district.");
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
