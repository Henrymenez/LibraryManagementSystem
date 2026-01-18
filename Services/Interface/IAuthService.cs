using LibraryManagementSystem.Dtos.Auth;

namespace LibraryManagementSystem.Services.Interface
{
    public interface IAuthService
    {
       Task<RegisterResponseDto> Register(RegisterRequestDto dto);
        Task<AuthResponseDto> Login(LoginRequestDto dto);
    }
}
