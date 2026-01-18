using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities
{
    public class Book
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;

        [Required, MaxLength(200)]
        public string Author { get; set; } = default!;

        [Required, MaxLength(30)]
        public string ISBN { get; set; } = default!;

        public DateTime PublishedDate { get; set; }
    }
}
