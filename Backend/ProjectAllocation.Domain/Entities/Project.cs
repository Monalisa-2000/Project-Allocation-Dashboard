namespace ProjectAllocation.Domain.Entities;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CreatedByAdminId { get; set; }
    public User CreatedByAdmin { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
}
