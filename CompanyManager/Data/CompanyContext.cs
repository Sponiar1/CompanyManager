using Microsoft.EntityFrameworkCore;
using CompanyManager.Models;

namespace CompanyManager.Data
{
    public class CompanyContext :DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {

        }

    }
}
