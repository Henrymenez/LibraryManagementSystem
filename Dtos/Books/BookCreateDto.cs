using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.Books
{
    public class BookCreateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;

        [Required, MaxLength(200)]
        public string Author { get; set; } = default!;

        [Required, MaxLength(30)]
        public string ISBN { get; set; } = default!;

        public DateTime PublishedDate { get; set; }
    }
}
