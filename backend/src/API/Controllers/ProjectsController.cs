using System.Security.Claims;
using Application.Common.IInterfaces;
using Application.DTOs.ProjectDTOs;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    private int GetWorkspaceId()
    {
        var claim = User.FindFirst("WorkspaceId");
        return claim != null ? int.Parse(claim.Value) : 0;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> Get()
    {
        var workspaceId = GetWorkspaceId();
        if (workspaceId == 0) return Unauthorized();
        
        var proyectos = await _projectService.GetProjectsByWorkspaceAsync(workspaceId);
        return Ok(proyectos);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.WorkspaceAdminEditor)]
    public async Task<ActionResult<ProjectDto>> Post([FromBody] CreateProjectDto dto)
    {
        var workspaceId = GetWorkspaceId();
        if (workspaceId == 0) return Unauthorized();
        
        var proyecto = await _projectService.CreateProjectAsync(workspaceId, dto);
        return CreatedAtAction(nameof(Get), new { id = proyecto.Id }, proyecto);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyNames.WorkspaceAdminEditor)]
    public async Task<ActionResult<ProjectDto>> Put(int id, [FromBody] UpdateProjectDto dto)
    {
        var workspaceId = GetWorkspaceId();
        if (workspaceId == 0) return Unauthorized();
        
        var proyecto = await _projectService.UpdateProjectAsync(id, dto.Nombre, workspaceId);
        if (proyecto == null) return NotFound();
        
        return Ok(proyecto);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> Delete(int id)
    {
        var workspaceId = GetWorkspaceId();
        if (workspaceId == 0) return Unauthorized();
        
        var result = await _projectService.DeleteProjectAsync(id, workspaceId);
        if (!result) return NotFound();
        
        return NoContent();
    }
}