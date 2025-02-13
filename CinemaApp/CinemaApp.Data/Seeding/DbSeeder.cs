using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Seeding
{
    using System.Text.Json;
    using CinemaApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using CinemaApp.Data.Seeding.DTOs;
    using Common;
    using CinemaApp.Services.Mapping;
    using System.Globalization;

    public static class DbSeeder
    {
        public static async Task SeedMoviesAsync(IServiceProvider serviceProvider, string jsonPath)
        {
            await using var dbContext = serviceProvider
                .GetRequiredService<CinemaDbContext>();

            var allMovies = await dbContext.Movies
                .ToArrayAsync();

            try
            {
                string jsonInput = await File
                    .ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                var movieDtos = JsonSerializer
                    .Deserialize<ImportMovieDto[]>(jsonInput);

                var moviesToAdd = new List<Movie>();

                foreach (var movieDto in movieDtos)
                {
                    if (!ValidationUtils.IsValid(movieDto))
                    {
                        continue;
                    }

                    var isMovieGuidValid = ValidationUtils.TryGetGuid(movieDto.Id, out Guid movieGuid);
                    if (!isMovieGuidValid)
                    {
                        continue;
                    }

                    bool isReleaseDateValid = DateTime
                        .TryParse(movieDto.ReleaseDate, CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime releaseDate);
                    if (!isReleaseDateValid)
                    {
                        continue;
                    }

                    if (allMovies.Any(m => 
                            m.Id.ToString().ToLowerInvariant() ==
                            movieGuid.ToString().ToLowerInvariant()))
                    {
                        continue;
                    }

                    var movie = AutoMapperConfig.MapperInstance.Map<Movie>(movieDto);
                    movie.ReleaseDate = releaseDate;

                    moviesToAdd.Add(movie);
                }

                if (moviesToAdd.Any())
                {
                    await dbContext.Movies.AddRangeAsync(moviesToAdd);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while seeding movies: {e.Message}");
                throw;
            }
        }
    }
}
