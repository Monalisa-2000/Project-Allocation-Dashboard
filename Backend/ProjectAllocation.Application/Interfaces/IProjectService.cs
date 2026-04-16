using ProjectAllocation.Application.DTOs.Projects;

namespace ProjectAllocation.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectResponse>> GetAllAsync();
    Task<ProjectResponse> GetByIdAsync(Guid id);
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request, Guid createdByAdminId);
    Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request);
    Task DeleteAsync(Guid id);
}
