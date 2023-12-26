using Microsoft.AspNetCore.Authorization;

public class AdultRequirement : IAuthorizationRequirement
{
    public int MinAge { get; } = 18;
}