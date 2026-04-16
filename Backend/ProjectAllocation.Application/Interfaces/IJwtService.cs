using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
