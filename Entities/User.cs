using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = default!;

        [Required]
        public byte[] PasswordHash { get; set; } = default!;

        [Required]
        public byte[] PasswordSalt { get; set; } = default!;
    }
}
