using Microsoft.EntityFrameworkCore;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Domain.Entities;
using ProjectAllocation.Infrastructure.Data;

namespace ProjectAllocation.Infrastructure.Repositories;

public class AllocationRepository : IAllocationRepository
{
    private readonly AppDbContext _context;

    public AllocationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Allocation>> GetAllAsync()
    {
        return await _context.Allocations
            .Include(x => x.User)
            .Include(x => x.Project)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Allocation>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Allocations
            .Where(x => x.UserId == userId)
            .Include(x => x.Project)
            .Include(x => x.User)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid projectId)
    {
        return await _context.Allocations.AnyAsync(x => x.UserId == userId && x.ProjectId == projectId);
    }

    public async Task<Allocation> CreateAsync(Allocation allocation)
    {
        _context.Allocations.Add(allocation);
        await _context.SaveChangesAsync();
        return await _context.Allocations
            .Include(x => x.User)
            .Include(x => x.Project)
            .FirstAsync(x => x.Id == allocation.Id);
    }

    public async Task<Allocation?> GetByIdAsync(Guid id)
    {
        return await _context.Allocations
            .Include(x => x.User)
            .Include(x => x.Project)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var allocation = await _context.Allocations.FindAsync(id);
        if (allocation is null)
        {
            return;
        }

        _context.Allocations.Remove(allocation);
        await _context.SaveChangesAsync();
    }

    public async Task<Allocation> UpdateAsync(Allocation allocation)
    {
        _context.Allocations.Update(allocation);
        await _context.SaveChangesAsync();

        return await _context.Allocations
            .Include(x => x.User)
            .Include(x => x.Project)
            .FirstAsync(x => x.Id == allocation.Id);
    }
}
