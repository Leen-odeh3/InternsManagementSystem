using IMS.Application.Abstractions;
using IMS.Application.DTOs.Users;
using IMS.Application.Mapper;
using IMS.Core.Constants;
using IMS.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IMS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    private readonly UserMapper _mapper;

    public AuthController(
        IAuthService authService,
        UserMapper mapper,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        _logger.LogInformation("Register request received for Email: {Email}", dto.Email);
        var appUser = _mapper.MapToAppUser(dto);

        Trainer? trainer = null;
        Trainee? trainee = null;

        if (string.Equals(dto.Role, StaticRole.Trainer, StringComparison.OrdinalIgnoreCase))
        {
            trainer = _mapper.MapToTrainer(dto);
            trainer.UserId = appUser.Id;
        }

        if (string.Equals(dto.Role, StaticRole.Trainee, StringComparison.OrdinalIgnoreCase))
        {
            trainee = _mapper.MapToTrainee(dto);
            trainee.UserId = appUser.Id;
        }

        var user = await _authService.AddNewUserAsync(
            appUser,
            dto.Password,
            dto.Role,
            trainer,
            trainee);
        _logger.LogInformation("User created successfully with Id: {UserId}", user.Id);
        var response = _mapper.MapToResponse(user);

        response.Role = dto.Role;

        return Ok(response);
    }
}
