using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}
