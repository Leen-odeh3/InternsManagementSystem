using IMS.Application.Abstractions;
using IMS.Application.DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
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
            var result = await _authService.AddNewUserAsync(userRequest);
            return CreatedAtAction(nameof(Register), new { id = result.UserId }, result);
        }
    }
}
