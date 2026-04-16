using ProjectAllocation.Application.DTOs.Allocations;

namespace ProjectAllocation.Application.Interfaces;

public interface IAllocationService
{
    Task<IEnumerable<AllocationResponse>> GetAllAsync();
    Task<IEnumerable<AllocationResponse>> AssignProjectsAsync(AssignRequest request);
    Task DeleteAllocationAsync(Guid id);
    Task<IEnumerable<AllocationResponse>> GetMyAllocationsAsync(Guid userId);
    Task<AllocationResponse> CompleteAllocationAsync(Guid allocationId, Guid requestingUserId);
}
