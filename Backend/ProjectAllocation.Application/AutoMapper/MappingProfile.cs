using AutoMapper;
using ProjectAllocation.Application.DTOs.Allocations;
using ProjectAllocation.Application.DTOs.Auth;
using ProjectAllocation.Application.DTOs.Projects;
using ProjectAllocation.Application.DTOs.Users;
using ProjectAllocation.Domain.Entities;

namespace ProjectAllocation.Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty));

        CreateMap<Project, ProjectResponse>();

        CreateMap<Allocation, AllocationResponse>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name));

        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty));
    }
}
