using Domain.Entities;

namespace Application.Common.IInterfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Proyecto>> GetByWorkspaceAsync(int workspaceId);
    Task<Proyecto> CreateAsync(Proyecto proyecto);
    Task<Proyecto?> GetByIdAsync(int id);
    Task<Proyecto> UpdateAsync(Proyecto proyecto);
    Task<bool> DeleteAsync(int id);
}