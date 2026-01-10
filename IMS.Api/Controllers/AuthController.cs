using IMS.Application.Abstractions;
using IMS.Application.DI;
using IMS.Application.DTOs.Users;
using IMS.Infrastructure.DependancyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService; 
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AppUserRequest userRequest)
    {
        using var activity =
            Observability.ActivitySource.StartActivity("Register User");

        activity?.SetTag("user.email", userRequest.Email);
        activity?.SetTag("user.role", userRequest.role);

        var result = await _authService.AddNewUserAsync(userRequest);

        activity?.SetTag("user.id", result.UserId);

        return CreatedAtAction(nameof(Register),
            new { id = result.UserId }, result);
    }
}
