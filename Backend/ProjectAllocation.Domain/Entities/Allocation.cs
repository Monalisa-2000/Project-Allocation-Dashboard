namespace ProjectAllocation.Domain.Entities;

public class Allocation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
