using AutoMapper;
using ProjectAllocation.Application.DTOs.Projects;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProjectResponse>>(projects);
    }

    public async Task<ProjectResponse> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Project not found.");
        return _mapper.Map<ProjectResponse>(project);
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, Guid createdByAdminId)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedByAdminId = createdByAdminId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _projectRepository.CreateAsync(project);
        return _mapper.Map<ProjectResponse>(created);
    }

    public async Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Project not found.");

        project.Name = request.Name;
        project.Description = request.Description;

        var updated = await _projectRepository.UpdateAsync(project);
        return _mapper.Map<ProjectResponse>(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Project not found.");
        await _projectRepository.DeleteAsync(project.Id);
    }
}
