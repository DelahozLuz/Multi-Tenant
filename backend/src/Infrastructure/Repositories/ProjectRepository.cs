using Application.Common.IInterfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Proyecto>> GetByWorkspaceAsync(int workspaceId)
    {
        return await _context.Proyectos
            .Where(p => p.WorkspaceId == workspaceId)
            .ToListAsync();
    }

    public async Task<Proyecto> CreateAsync(Proyecto proyecto)
    {
        _context.Proyectos.Add(proyecto);
        await _context.SaveChangesAsync();
        return proyecto;
    }

    public async Task<Proyecto?> GetByIdAsync(int id)
    {
        return await _context.Proyectos.FindAsync(id);
    }

    public async Task<Proyecto> UpdateAsync(Proyecto proyecto)
    {
        _context.Proyectos.Update(proyecto);
        await _context.SaveChangesAsync();
        return proyecto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto == null) return false;
        _context.Proyectos.Remove(proyecto);
        await _context.SaveChangesAsync();
        return true;
    }
}