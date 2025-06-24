using CompanyManager.Models;

namespace CompanyManager.Services.Templates
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task<Project> AddProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(int id, Project project);
        Task<bool> DeleteProjectAsync(int id);
    }
}
