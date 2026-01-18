using LibraryManagementSystem.Dtos.Books;
using LibraryManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpPost]
        public async Task<ActionResult<BookReadDto>> Create(BookCreateDto dto)
        {
            var result = await bookService.CreateBook(dto);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }


        [HttpGet]
        public async Task<ActionResult<PagedResultDto<BookReadDto>>> GetAll(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await bookService.GetAllBooks(search, pageNumber, pageSize);

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await bookService.GetBookById(id);
            if (result == null)
            {
                return NotFound(new { error = $"Book {id} not found." });
            }
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, BookUpdateDto dto)
        {
            var result = await bookService.UpdateBook(id, dto);
            if (result == null)
            {
                return NotFound(new { error = $"Book {id} not found." });
            }
            return Ok(result);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await bookService.DeleteBook(id);
            if (result == null)
            {
                return NotFound(new { error = $"Book {id} not found." });
            }
            return Ok(result);
        }
    }

}
