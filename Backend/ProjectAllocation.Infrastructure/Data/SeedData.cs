using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectAllocation.Domain.Entities;
using ProjectAllocation.Domain.Enums;

namespace ProjectAllocation.Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        await dbContext.Database.MigrateAsync();

        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var usersToSeed = new[]
        {
            new { Name = "Monalisa Khanda", Email = "monalisa@app.com", Role = UserRole.Admin, Password = "Admin@123" },
            new { Name = "Sooraj Singh",    Email = "sooraj@app.com",   Role = UserRole.User,  Password = "User@123"  },
            new { Name = "Priya Patel",     Email = "priya@app.com",    Role = UserRole.User,  Password = "User@123"  },
            new { Name = "Arjun Mehta",     Email = "arjun@app.com",    Role = UserRole.User,  Password = "User@123"  },
        };

        foreach (var seedUser in usersToSeed)
        {
            var existing = await userManager.FindByEmailAsync(seedUser.Email);
            if (existing is not null)
            {
                continue;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = seedUser.Name,
                UserName = seedUser.Email,
                Email = seedUser.Email,
                Role = seedUser.Role,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, seedUser.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, seedUser.Role.ToString());
            }
        }

        if (!await dbContext.Projects.AnyAsync())
        {
            var alice = await userManager.FindByEmailAsync("monalisa@app.com")
                ?? throw new InvalidOperationException("Monalisa Khanda was not seeded.");

            var projects = new[]
            {
                new Project { Id = Guid.NewGuid(), Name = "Website Redesign", Description = "Revamp the company public-facing website.", CreatedByAdminId = alice.Id, CreatedAt = DateTime.UtcNow },
                new Project { Id = Guid.NewGuid(), Name = "API Migration", Description = "Move legacy REST endpoints to GraphQL.", CreatedByAdminId = alice.Id, CreatedAt = DateTime.UtcNow },
                new Project { Id = Guid.NewGuid(), Name = "Mobile App MVP", Description = "Build the initial iOS and Android prototype.", CreatedByAdminId = alice.Id, CreatedAt = DateTime.UtcNow },
                new Project { Id = Guid.NewGuid(), Name = "Data Analytics Dashboard", Description = "Integrate BI reporting for the ops team.", CreatedByAdminId = alice.Id, CreatedAt = DateTime.UtcNow },
                new Project { Id = Guid.NewGuid(), Name = "Security Audit", Description = "Review and patch all known vulnerabilities.", CreatedByAdminId = alice.Id, CreatedAt = DateTime.UtcNow }
            };

            await dbContext.Projects.AddRangeAsync(projects);
            await dbContext.SaveChangesAsync();
        }
    }
}
