using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcProject2.Data;
using System;
using System.Linq;

namespace MvcMovie.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcProject2Context(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcProject2Context>>()))
        {
            // Look for any movies.
            if (context.Movie.Any())
            {
                return;   // DB has been seeded
            }
            context.Movie.AddRange(
                new Movie
                {
                    Title = "Pan Tadeusz",
                    ReleaseDate = "Adam Mickiewicz",
                    Genre = "Poezja",
                    Price = 7.99M,
                    Rating = "5"
                },
                new Movie
                {
                    Title = "Romeo i Julia",
                    ReleaseDate = "William Shakespeare",
                    Genre = "Dramat",
                    Price = 8.99M,
                    Rating = "3"
                },
                new Movie
                {
                    Title = "Hamlet",
                    ReleaseDate = "William Shakespeare",
                    Genre = "Dramat",
                    Price = 9.99M,
                    Rating = "4"
                },
                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = "Agatha Christie",
                    Genre = "Przygodowa",
                    Price = 3.99M,
                    Rating = "4.5"
                },
                new Movie
                {
                    Title = "Dziady",
                    ReleaseDate = "Adam Mickiewicz",
                    Genre = "Dramat",
                    Price = 7.99M,
                    Rating = "5"
                },
                new Movie
                {
                    Title = "Ballady i romanse",
                    ReleaseDate = "William Shakespeare",
                    Genre = "Romans",
                    Price = 8.99M,
                    Rating = "3"
                },
                new Movie
                {
                    Title = "Harry Potter: Kamien Filozoficzny",
                    ReleaseDate = "William Shakespeare",
                    Genre = "Fantasy",
                    Price = 9.99M,
                    Rating = "4"
                },
                new Movie
                {
                    Title = "Harry Potter: Komnata tajemnic",
                    ReleaseDate = "Agatha Christie",
                    Genre = "Fantasy",
                    Price = 3.99M,
                    Rating = "4.5"
                }
            );
            context.SaveChanges();
        }
    }
}