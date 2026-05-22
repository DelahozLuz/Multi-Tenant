namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public ICollection<UsuarioWorkspace> UsuarioWorkspaces { get; set; } = new List<UsuarioWorkspace>();
}