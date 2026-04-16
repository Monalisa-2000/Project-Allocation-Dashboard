using Microsoft.EntityFrameworkCore;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Domain.Entities;
using ProjectAllocation.Domain.Enums;
using ProjectAllocation.Infrastructure.Data;

namespace ProjectAllocation.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Where(x => x.Role == UserRole.User)
            .Include(x => x.Allocations)
            .ThenInclude(x => x.Project)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(x => x.Allocations)
            .ThenInclude(x => x.Project)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            return;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
