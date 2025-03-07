﻿namespace CinemaApp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    public class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<ApplicationUserMovie>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserMovie> builder)
        {
            builder
                .HasKey(um => new { um.ApplicationUserId, um.MovieId });

            builder
                .Property(um => um.IsDeleted)
                .HasDefaultValue(false);

            builder
                .HasOne(um => um.Movie)
                .WithMany(m => m.ApplicationUserMovies)
                .HasForeignKey(um => um.MovieId);

            builder
                .HasOne(um => um.ApplicationUser)
                .WithMany(u => u.ApplicationUserMovies)
                .HasForeignKey(um => um.ApplicationUserId);
        }
    }
}
