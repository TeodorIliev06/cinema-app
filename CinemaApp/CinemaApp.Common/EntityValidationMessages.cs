namespace CinemaApp.Common
{
    public class EntityValidationMessages
    {
        public class Movie
        {
            public const string TitleRequiredMessage = "Movie title is required.";
            public const string GenreRequiredMessage = "Genre is required.";
            public const string ReleaseDateRequiredMessage = "Release date is required in format MM/yyyy.";
            public const string DirectorRequiredMessage = "Director name is required.";
            public const string DurationRequiredMessage = "Please specify movie duration.";
        }
    }
}
