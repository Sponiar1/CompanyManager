using CompanyManager.Mappers;
using CompanyManager.Models;

namespace CompanyManager.Services.Templates
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        public Task<Employee> GetEmployeeByIdAsync(int id);
        public Task<Employee> AddEmployeeAsync(Employee employeeDto);
        public Task<Employee> UpdateEmployeeAsync(int id, Employee employeeDto);
        public Task<bool> DeleteEmployeeAsync(int id);
    }
}
