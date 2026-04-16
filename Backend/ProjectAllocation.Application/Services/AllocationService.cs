using AutoMapper;
using ProjectAllocation.Application.DTOs.Allocations;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Application.Services;

public class AllocationService : IAllocationService
{
    private readonly IAllocationRepository _allocationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public AllocationService(
        IAllocationRepository allocationRepository,
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _allocationRepository = allocationRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AllocationResponse>> AssignProjectsAsync(AssignRequest request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new KeyNotFoundException("User not found.");

        var responses = new List<AllocationResponse>();
        foreach (var projectId in request.ProjectIds.Distinct())
        {
            var project = await _projectRepository.GetByIdAsync(projectId)
                ?? throw new KeyNotFoundException("Project not found.");

            var exists = await _allocationRepository.ExistsAsync(request.UserId, projectId);
            if (exists)
            {
                throw new InvalidOperationException($"Project {project.Name} is already assigned to this user.");
            }

            var created = await _allocationRepository.CreateAsync(new Allocation
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ProjectId = project.Id,
                AssignedAt = DateTime.UtcNow,
                IsCompleted = false
            });

            responses.Add(_mapper.Map<AllocationResponse>(created));
        }

        return responses;
    }

    public async Task<IEnumerable<AllocationResponse>> GetAllAsync()
    {
        var allocations = await _allocationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AllocationResponse>>(allocations);
    }

    public async Task<IEnumerable<AllocationResponse>> GetMyAllocationsAsync(Guid userId)
    {
        var allocations = await _allocationRepository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<AllocationResponse>>(allocations);
    }

    public async Task<AllocationResponse> CompleteAllocationAsync(Guid allocationId, Guid requestingUserId)
    {
        var allocation = await _allocationRepository.GetByIdAsync(allocationId)
            ?? throw new KeyNotFoundException("Allocation not found.");

        if (allocation.UserId != requestingUserId)
        {
            throw new UnauthorizedAccessException("You can only complete your own allocation.");
        }

        allocation.IsCompleted = !allocation.IsCompleted;
        allocation.CompletedAt = allocation.IsCompleted ? DateTime.UtcNow : null;

        var updated = await _allocationRepository.UpdateAsync(allocation);
        return _mapper.Map<AllocationResponse>(updated);
    }

    public async Task DeleteAllocationAsync(Guid id)
    {
        var allocation = await _allocationRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Allocation not found.");
        await _allocationRepository.DeleteAsync(allocation.Id);
    }
}
