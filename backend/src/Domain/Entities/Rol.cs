namespace Domain.Entities;

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<UsuarioWorkspace> UsuarioWorkspaces { get; set; } = new List<UsuarioWorkspace>();
}