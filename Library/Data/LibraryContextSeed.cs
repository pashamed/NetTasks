using Library.Models;

namespace Library.Data
{
    public static class LibraryContextSeed
    {
        public static async Task SeedAsync(LibraryContext context)
        {
            if (!context.Genres.Any())
            {
                context.Genres.AddRange(new List<Genre>()
                {
                    new Genre()
                    {
                        Name = "Драма",

                    },
                    new Genre()
                    {
                        Name = "Триллер",

                    },
                    new Genre()
                    {
                        Name = "Биография",

                    },
                    new Genre()
                    {
                        Name = "История",

                    },
                    new Genre()
                    {
                        Name = "Ужасы",

                    },
                });
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}
