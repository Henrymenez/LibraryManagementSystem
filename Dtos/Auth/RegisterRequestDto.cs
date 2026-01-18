using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.Auth
{
    public class RegisterRequestDto
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Username { get; set; } = default!;

        [Required, MinLength(6), MaxLength(200)]
        public string Password { get; set; } = default!;
    }
}
