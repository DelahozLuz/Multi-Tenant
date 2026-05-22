using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<UsuarioWorkspace> UsuarioWorkspaces => Set<UsuarioWorkspace>();
    public DbSet<Proyecto> Proyectos => Set<Proyecto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- CONFIGURACIÓN DE ENTIDADES ---

        modelBuilder.Entity<Usuario>(entity => {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Password).HasMaxLength(100).IsRequired();

            // Seed de Usuarios
            entity.HasData(
                new Usuario { Id = 1, Email = "admin@gmail.com", Password = "$2a$11$ydqvSD7yQ88lg6kCm..wUuGLp1pMqiY7RJqoyOYpQd6x4t0uWEUl." },
                new Usuario { Id = 4, Email = "test1@ejemplo.com", Password = "$2a$11$ydqvSD7yQ88lg6kCm..wUuGLp1pMqiY7RJqoyOYpQd6x4t0uWEUl." }
            );
        });

        modelBuilder.Entity<Workspace>(entity => {
            entity.ToTable("Workspaces");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();

            // Seed de Workspaces
            entity.HasData(
                new Workspace { Id = 1, Nombre = "Alfa" },
                new Workspace { Id = 2, Nombre = "Beta" }
            );
        });

        modelBuilder.Entity<Rol>(entity => {
            entity.ToTable("Roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();

            // Seed de Roles
            entity.HasData(
                new Rol { Id = 1, Nombre = "Admin" },
                new Rol { Id = 2, Nombre = "Editor" },
                new Rol { Id = 3, Nombre = "Lector" }
            );
        });

        modelBuilder.Entity<UsuarioWorkspace>(entity => {
            entity.ToTable("Usuario_Workspace");
            entity.HasKey(e => new { e.UsuarioId, e.WorkspaceId });
            entity.HasOne(e => e.Usuario).WithMany(u => u.UsuarioWorkspaces).HasForeignKey(e => e.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Workspace).WithMany(w => w.UsuarioWorkspaces).HasForeignKey(e => e.WorkspaceId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Rol).WithMany(r => r.UsuarioWorkspaces).HasForeignKey(e => e.RolId).OnDelete(DeleteBehavior.Restrict);

            // Seed de Relación Usuario-Workspace
            entity.HasData(
                new UsuarioWorkspace { UsuarioId = 1, WorkspaceId = 1, RolId = 1 },
                new UsuarioWorkspace { UsuarioId = 1, WorkspaceId = 2, RolId =  3},
                new UsuarioWorkspace { UsuarioId = 4, WorkspaceId = 1, RolId =  2},
                new UsuarioWorkspace { UsuarioId = 4, WorkspaceId = 2, RolId = 3 }
            );
        });

        modelBuilder.Entity<Proyecto>(entity => {
            entity.ToTable("Proyectos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.HasOne(e => e.Workspace).WithMany(w => w.Proyectos).HasForeignKey(e => e.WorkspaceId).OnDelete(DeleteBehavior.Cascade);

            // Seed de Proyectos
            entity.HasData(
                new Proyecto { Id = 1, Nombre = "Proyecto Alpha", WorkspaceId = 1 },
                new Proyecto { Id = 2, Nombre = "Proyecto Alpha 2", WorkspaceId = 1 },
                new Proyecto { Id = 3, Nombre = "Proyecto Beta 1", WorkspaceId = 2 },
                new Proyecto { Id = 4, Nombre = "Proyecto Beta 2", WorkspaceId = 2 }
            );
        });
    }
}