namespace ProjectAllocation.Application.DTOs.Projects;

public class ProjectResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid CreatedByAdminId { get; set; }
    public DateTime CreatedAt { get; set; }
}
