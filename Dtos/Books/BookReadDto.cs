namespace LibraryManagementSystem.Dtos.Books
{
    public class BookReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public string ISBN { get; set; } = default!;
        public DateTime PublishedDate { get; set; }
    }
}
