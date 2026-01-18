using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
