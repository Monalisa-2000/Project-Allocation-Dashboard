using AutoMapper;
using Moq;
using ProjectAllocation.Application.AutoMapper;
using ProjectAllocation.Application.DTOs.Projects;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Application.Services;
using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Tests.Services;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepository = new();
    private readonly IMapper _mapper;

    public ProjectServiceTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public async Task CreateProject_ValidRequest_ReturnsProject()
    {
        var request = new CreateProjectRequest { Name = "Test Project", Description = "Desc" };
        _projectRepository.Setup(x => x.CreateAsync(It.IsAny<Project>())).ReturnsAsync((Project p) => p);
        var service = new ProjectService(_projectRepository.Object, _mapper);

        var result = await service.CreateAsync(request, Guid.NewGuid());

        Assert.Equal(request.Name, result.Name);
    }

    [Fact]
    public async Task GetById_NonExistentId_ThrowsNotFound()
    {
        _projectRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Project?)null);
        var service = new ProjectService(_projectRepository.Object, _mapper);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteProject_ValidId_CallsRepository()
    {
        var id = Guid.NewGuid();
        _projectRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(new Project { Id = id, Name = "X" });
        var service = new ProjectService(_projectRepository.Object, _mapper);

        await service.DeleteAsync(id);

        _projectRepository.Verify(x => x.DeleteAsync(id), Times.Once);
    }
}
