namespace Domain.Entities;

public class Proyecto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
}