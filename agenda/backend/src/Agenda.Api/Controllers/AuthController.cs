// Agenda.Api/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration cfg) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (dto.Email == "admin@local" && dto.Password == "Admin@123")
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(cfg["Jwt:Key"]!));
            var token = new JwtSecurityToken(
                claims: new[] { new Claim(ClaimTypes.Name, dto.Email) },
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        return Unauthorized();
    }
}

public record LoginDto(string Email, string Password);
