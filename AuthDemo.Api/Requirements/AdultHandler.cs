
using Microsoft.AspNetCore.Authorization;

public class AdultHandler : AuthorizationHandler<AdultRequirement>
{
    private readonly ILogger<AdultHandler> logger;

    public AdultHandler(ILogger<AdultHandler> logger)
    {
        this.logger = logger;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                        AdultRequirement requirement)
    {
        var age = context.User.FindFirst("Age");
        if(age is null) return Task.CompletedTask;
        var isOk = int.TryParse(age.Value, out var ageint);
        if(!isOk) return Task.CompletedTask;
        if(ageint >= requirement.MinAge)
        {
            context.Succeed(requirement);
        }
        else logger.LogError("Auth failed {0}", ageint);
        return Task.CompletedTask;

    }
}