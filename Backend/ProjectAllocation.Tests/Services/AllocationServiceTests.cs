using AutoMapper;
using Moq;
using ProjectAllocation.Application.AutoMapper;
using ProjectAllocation.Application.DTOs.Allocations;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Application.Services;
using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Tests.Services;

public class AllocationServiceTests
{
    private readonly Mock<IAllocationRepository> _allocationRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IProjectRepository> _projectRepository = new();
    private readonly IMapper _mapper;

    public AllocationServiceTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task AssignProjects_ValidRequest_ReturnsAllocations()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        _userRepository.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(new User { Id = userId, Name = "Bob" });
        _projectRepository.Setup(x => x.GetByIdAsync(projectId))
            .ReturnsAsync(new Project { Id = projectId, Name = "Website" });
        _allocationRepository.Setup(x => x.ExistsAsync(userId, projectId)).ReturnsAsync(false);
        _allocationRepository.Setup(x => x.CreateAsync(It.IsAny<Allocation>()))
            .ReturnsAsync((Allocation a) => new Allocation
            {
                Id = a.Id,
                UserId = a.UserId,
                ProjectId = a.ProjectId,
                User = new User { Id = userId, Name = "Bob" },
                Project = new Project { Id = projectId, Name = "Website" },
                AssignedAt = a.AssignedAt
            });

        var service = new AllocationService(_allocationRepository.Object, _userRepository.Object, _projectRepository.Object, _mapper);
        var result = await service.AssignProjectsAsync(new AssignRequest { UserId = userId, ProjectIds = new List<Guid> { projectId } });

        Assert.Single(result);
    }

    [Fact]
    public async Task AssignProjects_DuplicateAssignment_Throws409()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        _userRepository.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new User { Id = userId });
        _projectRepository.Setup(x => x.GetByIdAsync(projectId)).ReturnsAsync(new Project { Id = projectId, Name = "Website" });
        _allocationRepository.Setup(x => x.ExistsAsync(userId, projectId)).ReturnsAsync(true);

        var service = new AllocationService(_allocationRepository.Object, _userRepository.Object, _projectRepository.Object, _mapper);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.AssignProjectsAsync(new AssignRequest { UserId = userId, ProjectIds = new List<Guid> { projectId } }));
    }

    [Fact]
    public async Task AssignProjects_UserNotFound_Throws404()
    {
        _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
        var service = new AllocationService(_allocationRepository.Object, _userRepository.Object, _projectRepository.Object, _mapper);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.AssignProjectsAsync(new AssignRequest { UserId = Guid.NewGuid(), ProjectIds = new List<Guid> { Guid.NewGuid() } }));
    }

    [Fact]
    public async Task CompleteAllocation_ValidUser_TogglesCompletion()
    {
        var allocationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var allocation = new Allocation
        {
            Id = allocationId,
            UserId = userId,
            User = new User { Id = userId, Name = "Bob" },
            ProjectId = Guid.NewGuid(),
            Project = new Project { Id = Guid.NewGuid(), Name = "Website" },
            IsCompleted = false
        };

        _allocationRepository.Setup(x => x.GetByIdAsync(allocationId)).ReturnsAsync(allocation);
        _allocationRepository.Setup(x => x.UpdateAsync(It.IsAny<Allocation>())).ReturnsAsync((Allocation a) => a);
        var service = new AllocationService(_allocationRepository.Object, _userRepository.Object, _projectRepository.Object, _mapper);

        var result = await service.CompleteAllocationAsync(allocationId, userId);

        Assert.True(result.IsCompleted);
    }

    [Fact]
    public async Task CompleteAllocation_WrongUser_ThrowsUnauthorized()
    {
        var allocationId = Guid.NewGuid();
        _allocationRepository.Setup(x => x.GetByIdAsync(allocationId)).ReturnsAsync(new Allocation
        {
            Id = allocationId,
            UserId = Guid.NewGuid(),
            User = new User { Name = "A" },
            Project = new Project { Name = "P" }
        });

        var service = new AllocationService(_allocationRepository.Object, _userRepository.Object, _projectRepository.Object, _mapper);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.CompleteAllocationAsync(allocationId, Guid.NewGuid()));
    }
}
