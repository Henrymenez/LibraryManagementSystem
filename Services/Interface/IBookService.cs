using LibraryManagementSystem.Dtos.Books;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Services.Interface
{
    public interface IBookService
    {

      Task<BookReadDto> CreateBook(BookCreateDto dto);
        Task<BookReadDto?> UpdateBook(int id, BookUpdateDto dto);
       Task<PagedResultDto<BookReadDto>> GetAllBooks(string? search, int pageNumber = 1, int pageSize = 10);
        Task<BookReadDto?> GetBookById(int id);
        Task<string?> DeleteBook(int id);


    }
}
