using AutoMapper;
using LibraryManagementSystem.Dtos.Books;
using LibraryManagementSystem.Entities;

namespace LibraryManagementSystem.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookReadDto>();
            CreateMap<BookCreateDto, Book>();
            CreateMap<BookUpdateDto, Book>();
        }
    }
}
