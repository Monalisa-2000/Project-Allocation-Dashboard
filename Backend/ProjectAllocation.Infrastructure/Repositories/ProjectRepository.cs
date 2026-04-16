using Microsoft.EntityFrameworkCore;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Domain.Entities;
using ProjectAllocation.Infrastructure.Data;

namespace ProjectAllocation.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
        {
            return;
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
}
