using System.Globalization;

namespace LibraryManagementSystem.Dtos.Auth
{
    public class RegisterResponseDto
    {
      public string Message { get;set; } = default!;
        public string Username { get;set; } = default!;
        public bool ISuccessful { get;set; }
    }
}
