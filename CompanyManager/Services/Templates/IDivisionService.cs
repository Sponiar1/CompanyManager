using CompanyManager.Models;

namespace CompanyManager.Services.Templates
{
    public interface IDivisionService
    {
        Task<IEnumerable<Division>> GetAllDivisionsAsync();
        Task<Division> GetDivisionByIdAsync(int id);
        Task<Division> AddDivisionAsync(Division division);
        Task<Division> UpdateDivisionAsync(int id, Division division);
        Task<bool> DeleteDivisionAsync(int id);
    }
}
