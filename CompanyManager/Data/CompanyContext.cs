using Microsoft.EntityFrameworkCore;
using Company.Models;

namespace Company.Data
{
    public class CompanyContext :DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Company.Models.Company> Companies { get; set; }
        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {

        }

    }
}
