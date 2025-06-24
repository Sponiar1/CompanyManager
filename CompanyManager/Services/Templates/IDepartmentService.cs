using CompanyManager.Models;

namespace CompanyManager.Services.Templates
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<Department> AddDepartmentAsync(Department department);
        Task<Department> UpdateDepartmentAsync(int id, Department department);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
