using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Application.Interfaces;

public interface IAllocationRepository
{
    Task<IEnumerable<Allocation>> GetAllAsync();
    Task<IEnumerable<Allocation>> GetByUserIdAsync(Guid userId);
    Task<bool> ExistsAsync(Guid userId, Guid projectId);
    Task<Allocation> CreateAsync(Allocation allocation);
    Task<Allocation?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<Allocation> UpdateAsync(Allocation allocation);
}
