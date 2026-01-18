using LibraryManagementSystem.Data;
using LibraryManagementSystem.Dtos.Auth;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwt;

    public AuthController(AppDbContext db, IPasswordHasher hasher, IJwtTokenService jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto dto)
    {
        var exists = await _db.Users.AnyAsync(u => u.Username == dto.Username);
        if (exists) return Conflict(new { error = "Username already exists." });

        _hasher.CreatePasswordHash(dto.Password, out var hash, out var salt);

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
        if (user is null) return Unauthorized(new { error = "Invalid credentials." });

        var ok = _hasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);
        if (!ok) return Unauthorized(new { error = "Invalid credentials." });

        var token = _jwt.CreateToken(user);
        return Ok(new AuthResponseDto { Token = token });
    }

}
