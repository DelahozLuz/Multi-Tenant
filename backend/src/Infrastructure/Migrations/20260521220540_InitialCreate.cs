using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workspaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proyectos_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario_Workspace",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario_Workspace", x => new { x.UsuarioId, x.WorkspaceId });
                    table.ForeignKey(
                        name: "FK_Usuario_Workspace_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Workspace_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Workspace_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Editor" },
                    { 3, "Lector" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Password" },
                values: new object[,]
                {
                    { 1, "admin@gmail.com", "$2a$11$ydqvSD7yQ88lg6kCm..wUuGLp1pMqiY7RJqoyOYpQd6x4t0uWEUl." },
                    { 4, "test1@ejemplo.com", "$2a$11$ydqvSD7yQ88lg6kCm..wUuGLp1pMqiY7RJqoyOYpQd6x4t0uWEUl." }
                });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Alfa" },
                    { 2, "Beta" }
                });

            migrationBuilder.InsertData(
                table: "Proyectos",
                columns: new[] { "Id", "Nombre", "WorkspaceId" },
                values: new object[,]
                {
                    { 1, "Proyecto Alpha", 1 },
                    { 2, "Proyecto Alpha 2", 1 },
                    { 3, "Proyecto Beta 1", 2 },
                    { 4, "Proyecto Beta 2", 2 }
                });

            migrationBuilder.InsertData(
                table: "Usuario_Workspace",
                columns: new[] { "UsuarioId", "WorkspaceId", "RolId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 1, 2, 3 },
                    { 4, 1, 2 },
                    { 4, 2, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_WorkspaceId",
                table: "Proyectos",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Workspace_RolId",
                table: "Usuario_Workspace",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Workspace_WorkspaceId",
                table: "Usuario_Workspace",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Usuario_Workspace");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Workspaces");
        }
    }
}
