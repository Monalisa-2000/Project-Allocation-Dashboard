using ProjectAllocation.Application.DTOs.Allocations;

namespace ProjectAllocation.Application.DTOs.Users;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<AllocationResponse> Allocations { get; set; } = new();
}
