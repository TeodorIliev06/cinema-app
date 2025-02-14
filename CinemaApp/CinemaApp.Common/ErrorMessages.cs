using System.Numerics;
using static CinemaApp.Common.ErrorMessages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CinemaApp.Common
{
    public static class ErrorMessages
    {
        public static class Watchlist
        {
            public const string AddToWatchlistNotSuccessfulMessage = "Movie can not be added to watchlist. Please check if the movie is not already in your watchlist!";
            public const string RemoveFromWatchlistNotSuccessfulMessage = "Unexpected error occurred while removing the movie from your watchlist!";
        }

        public static class Cinema
        {
            public const string RemoveFromCinemaNotSuccessfulMessage = "An unexpected error occurred while deleting the cinema. Please try again later!";
            public const string EditCinemaNotSuccessfulMessage = "Unexpected error occured while trying to update the cinema! Please contact an administrator.";
            public static readonly string CinemaSeedFail = "Error occurred while seeding cinemas: {0}";
        }

        public static class CinemaMovie
        {
            public const string AvailableTicketsRequiredMessage = "Please enter the number of available tickets.";
            public const string AvailableTicketsRangeMessage = "Available tickets must be a positive number.";
        }

        public static class Movie
        {
            public const string RemoveFromMovieNotSuccessfulMessage = "An unexpected error occurred while deleting the movie. Please try again later!";
            public const string EditMovieNotSuccessfulMessage = "Unexpected error occured while trying to update the movie! Please contact an administrator.";
            public static readonly string MovieSeedFail = "Error occurred while seeding movies: {0}";
        }

        public static class Ticket
        {
            public const string InvalidTicketsCount = "Tickets count should be a positive number!";
            public const string InvalidTicketPrice = "Ticket price should be positive";
        }

        public static class Seeding
        {
            public static readonly string InvalidMethodName = "Seeding method {0} not found in DbSeeder";
            public static readonly string ExecutionError = "Error executing {0}: {1}";
        }
    }
}
