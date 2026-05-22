namespace Domain.Entities;

public class UsuarioWorkspace
{
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public int WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
    public int RolId { get; set; }
    public Rol Rol { get; set; } = null!;
}