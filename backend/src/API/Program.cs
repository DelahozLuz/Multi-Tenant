using System.Text;
using Application.Common.IInterfaces;
using Application.Services;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKey123456789012345678901234567890";
var tokenExpirationHours = int.Parse(builder.Configuration["Jwt:Hours"] ?? "8");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddSingleton<TempTokenService>();

builder.Services.AddScoped<IAuthService>(sp =>
    new AuthService(
        sp.GetRequiredService<IAuthRepository>(),
        sp.GetRequiredService<TempTokenService>(),
        jwtKey,
        tokenExpirationHours));
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "SaaSMultiTenant",
            ValidAudience = "SaaSMultiTenant",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.WorkspaceAdminEditor, policy =>
        policy.Requirements.Add(new WorkspaceAuthorizeRequirement("Admin", "Editor")));
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});
builder.Services.AddScoped<IAuthorizationHandler, WorkspaceAuthorizeHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SaaS Multi-Tenant API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SaaS Multi-Tenant API v1"));

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (await dbContext.Database.CanConnectAsync())
    {
        await dbContext.Database.MigrateAsync();
    }
    else
    {
        await dbContext.Database.MigrateAsync();
    }
}
app.Run();