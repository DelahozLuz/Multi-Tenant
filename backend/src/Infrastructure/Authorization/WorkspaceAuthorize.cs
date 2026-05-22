using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public class WorkspaceAuthorizeRequirement : IAuthorizationRequirement
{
    public string[] AllowedRoles { get; }
    public WorkspaceAuthorizeRequirement(params string[] allowedRoles)
    {
        AllowedRoles = allowedRoles;
    }
}

public class WorkspaceAuthorizeHandler : AuthorizationHandler<WorkspaceAuthorizeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        WorkspaceAuthorizeRequirement requirement)
    {
        var workspaceClaim = context.User.FindFirst("WorkspaceId");
        var roleClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.Role);

        if (workspaceClaim == null || roleClaim == null)
        {
            return Task.CompletedTask;
        }

        if (requirement.AllowedRoles.Contains(roleClaim.Value))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public static class PolicyNames
{
    public const string WorkspaceAdminEditor = "WorkspaceAdminEditor";
}