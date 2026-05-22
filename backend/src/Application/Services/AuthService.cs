using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.IInterfaces;
using Application.DTOs.AuthDTOs;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly TempTokenService _tempTokenService;
    private readonly string _jwtKey;
    private readonly int _tokenExpirationHours;

    public AuthService(IAuthRepository authRepository, TempTokenService tempTokenService, string jwtKey, int tokenExpirationHours = 8)
    {
        _authRepository = authRepository;
        _tempTokenService = tempTokenService;
        _jwtKey = jwtKey;
        _tokenExpirationHours = tokenExpirationHours;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _authRepository.GetByEmailAsync(dto.Email);
        if (usuario == null) return null;

        var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password);
        if (!passwordValid && usuario.Password != dto.Password)
            return null;

        var usuarioConWorkspaces = await _authRepository.GetByIdWithWorkspacesAsync(usuario.Id);
        if (usuarioConWorkspaces == null) return null;

        var workspaces = usuarioConWorkspaces.UsuarioWorkspaces.Select(uw => new WorkspaceRolDto
        {
            WorkspaceId = uw.WorkspaceId,
            NombreWorkspace = uw.Workspace.Nombre,
            Rol = uw.Rol.Nombre
        }).ToList();

        var tempToken = _tempTokenService.GenerateTempToken(usuario.Id);

        return new LoginResponseDto
        {
            UsuarioId = usuario.Id,
            Email = usuario.Email,
            TempToken = tempToken,
            Workspaces = workspaces
        };
    }

    public async Task<TokenResponseDto?> GenerateTokenAsync(TokenRequestDto dto)
    {
        var tempInfo = _tempTokenService.ValidateTempToken(dto.TempToken);
        if (tempInfo == null) return null;

        var usuarioConWorkspaces = await _authRepository.GetByIdWithWorkspacesAsync(tempInfo.UsuarioId);
        if (usuarioConWorkspaces == null) return null;

        var usuarioWorkspace = usuarioConWorkspaces.UsuarioWorkspaces
            .FirstOrDefault(uw => uw.WorkspaceId == dto.WorkspaceId);
        if (usuarioWorkspace == null) return null;

        var expiration = DateTime.UtcNow.AddHours(_tokenExpirationHours);
        var token = GenerateJwtToken(tempInfo.UsuarioId, dto.WorkspaceId, usuarioWorkspace.Rol.Nombre, expiration);

        return new TokenResponseDto
        {
            Token = token,
            WorkspaceId = dto.WorkspaceId,
            Rol = usuarioWorkspace.Rol.Nombre,
            Expiration = expiration
        };
    }

    private string GenerateJwtToken(int userId, int workspaceId, string role, DateTime expiration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("UserId", userId.ToString()),
            new Claim("WorkspaceId", workspaceId.ToString()),
            new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "SaaSMultiTenant",
            audience: "SaaSMultiTenant",
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}