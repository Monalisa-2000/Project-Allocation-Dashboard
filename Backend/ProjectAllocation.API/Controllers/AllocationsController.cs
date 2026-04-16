using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAllocation.Application.DTOs.Allocations;
using ProjectAllocation.Application.Interfaces;

namespace ProjectAllocation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AllocationsController : ControllerBase
{
    private readonly IAllocationService _allocationService;

    public AllocationsController(IAllocationService allocationService)
    {
        _allocationService = allocationService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _allocationService.GetAllAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Assign([FromBody] AssignRequest request)
    {
        var created = await _allocationService.AssignProjectsAsync(request);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _allocationService.DeleteAllocationAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "User")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyAllocations()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        return Ok(await _allocationService.GetMyAllocationsAsync(userId));
    }

    [Authorize(Roles = "User")]
    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var updated = await _allocationService.CompleteAllocationAsync(id, userId);
        return Ok(updated);
    }
}
