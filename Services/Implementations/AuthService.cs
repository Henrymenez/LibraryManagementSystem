using LibraryManagementSystem.Data;
using LibraryManagementSystem.Dtos.Auth;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenService _jwt;
        public AuthService(AppDbContext db, IPasswordHasher hasher, IJwtTokenService jwt)
        {
            _db = db;
            _hasher = hasher;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> Login(LoginRequestDto dto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
            if (user is null) return new AuthResponseDto { Message = "Invalid credentials.", ISuccessful = false };

            var ok = _hasher.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);
            if (!ok) return new AuthResponseDto { Message = "Invalid credentials.", ISuccessful = false };

            var token = _jwt.CreateToken(user);
            return new AuthResponseDto { Message = "Successful", Token = token , ISuccessful = true};
        }

        public async Task<RegisterResponseDto> Register(RegisterRequestDto dto)
        {
            var exists = await _db.Users.AnyAsync(u => u.Username == dto.Username);
            if (exists) return new RegisterResponseDto { Message = "Username already exists.", ISuccessful = false };

            _hasher.CreatePasswordHash(dto.Password, out var hash, out var salt);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return new RegisterResponseDto
            {
                Message = "User registered successfully.",
                Username = user.Username,
                ISuccessful = true
            };
        }
    }
}
