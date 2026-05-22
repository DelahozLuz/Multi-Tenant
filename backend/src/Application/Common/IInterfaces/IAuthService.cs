using Application.DTOs.AuthDTOs;

namespace Application.Common.IInterfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    Task<TokenResponseDto?> GenerateTokenAsync(TokenRequestDto dto);
}