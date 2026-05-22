namespace Application.DTOs.ProjectDTOs;

public class CreateProjectDto
{
    public string Nombre { get; set; } = string.Empty;
}

public class ProjectDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
}