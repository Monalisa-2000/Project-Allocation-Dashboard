using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Allocation> Allocations => Set<Allocation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Role).HasConversion<string>().IsRequired();
        });

        builder.Entity<Project>(entity =>
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasOne(x => x.CreatedByAdmin)
                .WithMany(x => x.CreatedProjects)
                .HasForeignKey(x => x.CreatedByAdminId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Allocation>(entity =>
        {
            entity.HasIndex(x => new { x.UserId, x.ProjectId }).IsUnique();
            entity.Property(x => x.IsCompleted).HasDefaultValue(false);

            entity.HasOne(x => x.User)
                .WithMany(x => x.Allocations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Project)
                .WithMany(x => x.Allocations)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        var utcConverter = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableUtcConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)) : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        builder.Entity<User>().Property(x => x.CreatedAt).HasConversion(utcConverter);
        builder.Entity<Project>().Property(x => x.CreatedAt).HasConversion(utcConverter);
        builder.Entity<Allocation>().Property(x => x.AssignedAt).HasConversion(utcConverter);
        builder.Entity<Allocation>().Property(x => x.CompletedAt).HasConversion(nullableUtcConverter);
    }
}
