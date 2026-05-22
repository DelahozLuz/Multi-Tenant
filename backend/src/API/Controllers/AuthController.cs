using Application.Common.IInterfaces;
using Application.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null) return Unauthorized(new { message = "Credenciales inválidas" });
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<ActionResult<TokenResponseDto>> GetToken([FromBody] TokenRequestDto dto)
    {
        var result = await _authService.GenerateTokenAsync(dto);
        if (result == null) return Unauthorized(new { message = "Token inválido o workspace no asignado" });
        return Ok(result);
    }

}