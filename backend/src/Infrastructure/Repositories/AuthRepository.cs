using Application.Common.IInterfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario?> GetByIdWithWorkspacesAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.UsuarioWorkspaces)
                .ThenInclude(uw => uw.Workspace)
            .Include(u => u.UsuarioWorkspaces)
                .ThenInclude(uw => uw.Rol)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }
}