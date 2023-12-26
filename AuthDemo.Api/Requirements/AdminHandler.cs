using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

public class AdminHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                    RoleRequirement requirement)
    {
        var user = context.User;
        var claim = user.FindFirst(ClaimTypes.Role);
        if (claim != null)
        {
            var roleName = claim.Value;
            if(roleName == requirement.RoleName)
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}