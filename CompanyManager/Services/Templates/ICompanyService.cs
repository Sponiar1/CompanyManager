using CompanyManager.Mappers;
using CompanyManager.Models;

namespace CompanyManager.Services.Templates
{
    public interface ICompanyService
    {
        public Task<IEnumerable<Company>> GetAllCompaniesAsync();
        public Task<Company> GetCompanyByIdAsync(int id);
        public Task<Company> AddCompanyAsync(Company companyDto);
        public Task<Company> UpdateCompanyAsync(int id, Company companyDto);
        public Task<bool> DeleteCompanyAsync(int id);
    }
}
