using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Dtos.Books;
using LibraryManagementSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public BooksController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // POST /api/books
        [HttpPost]
        public async Task<ActionResult<BookReadDto>> Create(BookCreateDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            var result = _mapper.Map<BookReadDto>(book);
            return CreatedAtAction(nameof(GetAll), new { id = book.Id }, result);
        }

        // GET /api/books?search=...&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<BookReadDto>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
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

            return Ok(new PagedResultDto<BookReadDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total,
                Items = items
            });
        }

        // PUT /api/books/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, BookUpdateDto dto)
        {
            var book = await _db.Books.SingleOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound(new { error = $"Book {id} not found." });

            _mapper.Map(dto, book);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE /api/books/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.SingleOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound(new { error = $"Book {id} not found." });

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }

}
