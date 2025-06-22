using CompanyManager.Data;
using CompanyManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Services
{
    public class DepartmentService
    {
        private readonly CompanyContext _context;
        public DepartmentService(CompanyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }
        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }
        public async Task<Department> AddDepartmentAsync(Department department)
        {
            var boss = await _context.Employees.FindAsync(department.Id_Boss);
            if (boss == null)
            {
                throw new Exception("Database update failed: Employee (Boss) does not exist.");
            }
            var project = await _context.Projects.FindAsync(department.Id_Project);
            if (project == null)
            {
                throw new Exception("Database update failed: Project does not exist.");
            }
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return department;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<Department> UpdateDepartmentAsync(int id, Department department)
        {
            var original = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id_Department == id);
            if (original == null)
            {
                throw new Exception("Database update failed: Department does not exist.");
            }
            var boss = await _context.Employees.FindAsync(department.Id_Boss);
            if (boss == null)
            {
                throw new Exception("Database update failed: Employee (Boss) does not exist.");
            }
            var project = await _context.Projects.FindAsync(department.Id_Project);
            if (project == null)
            {
                throw new Exception("Database update failed: Project does not exist.");
            }
            try
            {
                _context.Entry(department).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return department;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
