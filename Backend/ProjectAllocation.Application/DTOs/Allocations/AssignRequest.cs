namespace ProjectAllocation.Application.DTOs.Allocations;

public class AssignRequest
{
    public Guid UserId { get; set; }
    public List<Guid> ProjectIds { get; set; } = new();
}
