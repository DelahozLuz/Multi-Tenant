namespace Application.DTOs.AuthDTOs;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class WorkspaceRolDto
{
    public int WorkspaceId { get; set; }
    public string NombreWorkspace { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public int UsuarioId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string TempToken { get; set; } = string.Empty;
    public List<WorkspaceRolDto> Workspaces { get; set; } = new();
}

public class TokenRequestDto
{
    public int WorkspaceId { get; set; }
    public string TempToken { get; set; } = string.Empty;
}

public class TokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public int WorkspaceId { get; set; }
    public string Rol { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}