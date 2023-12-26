using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthDemo.Token;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthModel _authConfiguration;
    private readonly List<User> _users;

    public AuthController(IOptions<AuthModel> optionsAuth)
    {
        _users = new List<User>
        {
            new User("a@a.com", "Admin", 25, "Andres"),
            new User("b@b.com", "RRHH", 15, "Nestor")
        };
        _authConfiguration = optionsAuth.Value;
    }

    [HttpGet("authorize")]
    public async Task<IActionResult> GenerateTokenAsync(string email, CancellationToken cancellationToken)
    {
        var user = _users.SingleOrDefault(p => p.Email == email);

        if(user is null)
        {
            return NotFound();
        }

        var secret = _authConfiguration.SecretKey;
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

        var issuer = _authConfiguration.Issuer;
        var audience = _authConfiguration.Audience;

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Age", user.Age.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new
        {
            Token = tokenHandler.WriteToken(token),
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });
    }

    public record User(string Email, string Role, int Age, string  Name);
}