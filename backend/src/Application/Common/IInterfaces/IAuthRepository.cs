using Domain.Entities;

namespace Application.Common.IInterfaces;

public interface IAuthRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario?> GetByIdWithWorkspacesAsync(int id);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<bool> EmailExistsAsync(string email);
    Task<List<Usuario>> GetAllAsync();
}