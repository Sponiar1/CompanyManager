using CompanyManager.Data;
using CompanyManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompanyManager.Services
{
    public class ProjectService
    {
        private readonly CompanyContext _context;
        public ProjectService(CompanyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            try
            {
                return await _context.Projects.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load projects from database: " + ex.Message);
            }
        }
        public async Task<Project> GetProjectByIdAsync(int id)
        {
            try
            {
                return await _context.Projects.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load project from database: " + ex.Message);
            }
        }
        public async Task<Project> AddProjectAsync(Project project)
        {
            var boss = await _context.Employees.FindAsync(project.Id_Boss);
            if (boss == null)
            {
                throw new ArgumentException("Database update failed: Employee (Boss) does not exist.");
            }
            var division = await _context.Divisions.FindAsync(project.Id_Division);
            if (division == null)
            {
                throw new ArgumentException("Database update failed: Division does not exist.");
            }
            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<Project> UpdateProjectAsync(int id, Project project)
        {
            var original = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id_Project == id);
            if (original == null)
            {
                throw new ArgumentException("Database update failed: Project does not exist.");
            }
            var boss = await _context.Employees.FindAsync(project.Id_Boss);
            if (boss == null)
            {
                throw new ArgumentException("Database update failed: Employee (Boss) does not exist.");
            }
            var division = await _context.Divisions.FindAsync(project.Id_Division);
            if (division == null)
            {
                throw new ArgumentException("Database update failed: Division does not exist.");
            }
            try
            {
                _context.Entry(project).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                throw new Exception("Database update failed: " + ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.Include(p => p.Departments)
                                                .FirstOrDefaultAsync(p => p.Id_Project == id);
            if(project == null)
            {
                return false;
            }
            if (!project.Departments.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Project has associated departments and cannot be deleted.");
            }
            try
            {
                _context.Projects.Remove(project);
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
