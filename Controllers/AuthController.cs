using LibraryManagementSystem.Dtos.Auth;
using LibraryManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        var result = await authService.Register(dto);

        if (!result.ISuccessful)
        {
            return BadRequest(result);
        }
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto)
    {
        var result = await authService.Login(dto);
        if (!result.ISuccessful)
        {
            return Unauthorized(result);
        }
        return Ok(result);
    }

}
