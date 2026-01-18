using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Dtos.Books;
using LibraryManagementSystem.Entities;
using LibraryManagementSystem.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public BookService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BookReadDto> CreateBook(BookCreateDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            var result = _mapper.Map<BookReadDto>(book);
            return result;
        }

        public async Task<string?> DeleteBook(int id)
        {
            var book = await _db.Books.SingleOrDefaultAsync(b => b.Id == id);
            if (book is null) return null;

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return "deleted";
        }

        public async Task<PagedResultDto<BookReadDto>> GetAllBooks(string? search, int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 100);

            var query = _db.Books.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(b =>
                    b.Title.Contains(s) ||
                    b.Author.Contains(s));
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(b => b.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BookReadDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResultDto<BookReadDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            };
        }

        public async Task<BookReadDto?> GetBookById(int id)
        {
            var book = await _db.Books.SingleOrDefaultAsync(b => b.Id == id);
            if (book is null) return null;
            return new BookReadDto
            {
                Id = book.Id,
                Author = book.Author,
                ISBN = book.ISBN,
                PublishedDate = book.PublishedDate,
                Title = book.Title
            };
        }

        public async Task<BookReadDto?> UpdateBook(int id, BookUpdateDto dto)
        {
            var book = await _db.Books.SingleOrDefaultAsync(b => b.Id == id);
            if (book is null) return null;

            _mapper.Map(dto, book);
            await _db.SaveChangesAsync();
            return _mapper.Map<BookReadDto>(book);
        }
    }
}
