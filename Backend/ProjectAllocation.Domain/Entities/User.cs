using Microsoft.AspNetCore.Identity;
using ProjectAllocation.Domain.Enums;

namespace ProjectAllocation.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
    public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
}
