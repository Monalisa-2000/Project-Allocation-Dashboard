using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProjectAllocation.Application.DTOs.Users;
using ProjectAllocation.Application.Interfaces;
using ProjectAllocation.Domain.Entities;
using ProjectAllocation.Domain.Enums;

namespace ProjectAllocation.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, UserManager<User> userManager, IMapper mapper)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<IEnumerable<UserResponse>>(users.Where(u => u.Role == UserRole.User));
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("User not found.");
        return _mapper.Map<UserResponse>(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString())
            ?? throw new KeyNotFoundException("User not found.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }
}
