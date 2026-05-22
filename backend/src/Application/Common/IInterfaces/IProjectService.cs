using Application.DTOs.ProjectDTOs;

namespace Application.Common.IInterfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetProjectsByWorkspaceAsync(int workspaceId);
    Task<ProjectDto> CreateProjectAsync(int workspaceId, CreateProjectDto dto);
    Task<ProjectDto?> UpdateProjectAsync(int projectId, string nombre, int workspaceId);
    Task<bool> DeleteProjectAsync(int projectId, int workspaceId);
}