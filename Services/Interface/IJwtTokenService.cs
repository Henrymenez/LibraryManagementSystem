using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Services.Interface
{
    public interface IJwtTokenService
    {
        string CreateToken(User user);
    }
}
