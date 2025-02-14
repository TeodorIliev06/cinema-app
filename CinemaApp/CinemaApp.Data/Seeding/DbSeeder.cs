using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Seeding
{
    using System.Text.Json;
    using System.Globalization;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Common;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;
    using CinemaApp.Data.Seeding.DTOs;

    public static class DbSeeder
    {
        public static async Task SeedMoviesAsync(CinemaDbContext dbContext, string jsonPath)
        {
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
                Console.WriteLine(ErrorMessages.Movie.MovieSeedFail, e.Message);
                throw;
            }
        }

        public static async Task SeedCinemasAsync(CinemaDbContext dbContext, string jsonPath)
        {
            var allCinemas = await dbContext.Cinemas
                .ToArrayAsync();

            try
            {
                string jsonInput = await File
                    .ReadAllTextAsync(jsonPath, Encoding.ASCII, CancellationToken.None);

                var cinemaDtos = JsonSerializer
                    .Deserialize<ImportCinemaDto[]>(jsonInput);

                var cinemasToAdd = new List<Cinema>();

                foreach (var cinemaDto in cinemaDtos)
                {
                    if (!ValidationUtils.IsValid(cinemaDto))
                    {
                        continue;
                    }

                    var isCinemaGuidValid = ValidationUtils.TryGetGuid(cinemaDto.Id, out Guid cinemaGuid);
                    if (!isCinemaGuidValid)
                    {
                        continue;
                    }

                    if (allCinemas.Any(m =>
                            m.Id.ToString().ToLowerInvariant() ==
                            cinemaGuid.ToString().ToLowerInvariant()))
                    {
                        continue;
                    }

                    var cinema = AutoMapperConfig.MapperInstance.Map<Cinema>(cinemaDto);

                    cinemasToAdd.Add(cinema);
                }

                if (cinemasToAdd.Any())
                {
                    await dbContext.Cinemas.AddRangeAsync(cinemasToAdd);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(ErrorMessages.Cinema.CinemaSeedFail, e.Message);
                throw;
            }
        }
    }
}
