using System.Globalization;

namespace LibraryManagementSystem.Dtos.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool ISuccessful { get; set; }
    }
}
