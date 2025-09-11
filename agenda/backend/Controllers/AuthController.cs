using Agenda.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(TokenService tokenService) : ControllerBase
{
    public record LoginRequest(string Email, string Password);
    public record LoginResponse(string Token);

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
    {
        var token = tokenService.Create(req.Email); // demo simples
        return Ok(new LoginResponse(token));
    }
}
