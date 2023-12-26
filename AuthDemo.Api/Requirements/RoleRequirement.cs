using Microsoft.AspNetCore.Authorization;

public class RoleRequirement : IAuthorizationRequirement
{
    public string RoleName { get;  set; }

    public RoleRequirement(string role)
    {
        this.RoleName = role;
    }
}