namespace Domain.Entities;

public class Workspace
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<UsuarioWorkspace> UsuarioWorkspaces { get; set; } = new List<UsuarioWorkspace>();
    public ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}