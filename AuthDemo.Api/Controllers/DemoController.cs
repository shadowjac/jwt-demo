using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
[Authorize]

public class DemoController : ControllerBase
{
    List<User> users = new List<User>
    {
        new User("Andres", "Casas", "a@a.com"),
        new User("Pepito", "Perez", "b@b.com")
    };

    [HttpGet("users")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(users);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        var currentUser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if(currentUser is null)
        {
            return NotFound();
        }

        var user = users.Find(p => p.Email == currentUser.Value);

        if(user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [Authorize(Policy = "AdultPolicy")]
    [HttpGet("hello")]
    public async Task<IActionResult> SayHello(string name)
    {
        return Ok($"Hello: {name}");
    }

}