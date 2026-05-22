using Application.Common.IInterfaces;
using Application.DTOs.ProjectDTOs;
using Domain.Entities;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsByWorkspaceAsync(int workspaceId)
    {
        var proyectos = await _projectRepository.GetByWorkspaceAsync(workspaceId);
        return proyectos.Select(p => new ProjectDto
        {
            Id = p.Id,
            Nombre = p.Nombre,
            WorkspaceId = p.WorkspaceId
        });
    }

    public async Task<ProjectDto> CreateProjectAsync(int workspaceId, CreateProjectDto dto)
    {
        var proyecto = new Proyecto { Nombre = dto.Nombre, WorkspaceId = workspaceId };
        var created = await _projectRepository.CreateAsync(proyecto);
        return new ProjectDto { Id = created.Id, Nombre = created.Nombre, WorkspaceId = created.WorkspaceId };
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int projectId, string nombre, int workspaceId)
    {
        var proyecto = await _projectRepository.GetByIdAsync(projectId);
        if (proyecto == null || proyecto.WorkspaceId != workspaceId) return null;

        proyecto.Nombre = nombre;
        await _projectRepository.UpdateAsync(proyecto);
        return new ProjectDto { Id = proyecto.Id, Nombre = proyecto.Nombre, WorkspaceId = proyecto.WorkspaceId };
    }

    public async Task<bool> DeleteProjectAsync(int projectId, int workspaceId)
    {
        var proyecto = await _projectRepository.GetByIdAsync(projectId);
        if (proyecto == null || proyecto.WorkspaceId != workspaceId) return false;

        return await _projectRepository.DeleteAsync(projectId);
    }
}