using ProjectAllocation.Application.DTOs.Users;

namespace ProjectAllocation.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}
