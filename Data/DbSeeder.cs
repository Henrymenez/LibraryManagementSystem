using LibraryManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{

    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            await db.Database.MigrateAsync();

            if (!await db.Books.AnyAsync())
            {
                db.Books.AddRange(
                    new Book
                    {
                        Title = "Clean Code",
                        Author = "Robert C. Martin",
                        ISBN = "9780132350884",
                        PublishedDate = new DateTime(2008, 8, 1)
                    },
                    new Book
                    {
                        Title = "The Pragmatic Programmer",
                        Author = "Andrew Hunt, David Thomas",
                        ISBN = "9780201616224",
                        PublishedDate = new DateTime(1999, 10, 30)
                    },
                    new Book
                    {
                        Title = "Design Patterns",
                        Author = "Erich Gamma et al.",
                        ISBN = "9780201633610",
                        PublishedDate = new DateTime(1994, 10, 31)
                    }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
