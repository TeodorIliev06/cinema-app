﻿// <auto-generated />
using System;
using CinemaApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CinemaApp.Data.Migrations
{
    [DbContext(typeof(CinemaDbContext))]
    [Migration("20250119222231_SoftDeleteCinemasMovies")]
    partial class SoftDeleteCinemasMovies
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CinemaApp.Data.Models.Cinema", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(85)
                        .HasColumnType("nvarchar(85)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Cinemas");

                    b.HasData(
                        new
                        {
                            Id = new Guid("fca6473b-091d-4a4e-855e-0029539020b5"),
                            Location = "123 Main Street, Springfield",
                            Name = "Starlight Cinema"
                        },
                        new
                        {
                            Id = new Guid("69a42c94-bdfb-430a-8caf-35a23d17e099"),
                            Location = "456 Elm Street, Metropolis",
                            Name = "Galaxy Theaters"
                        });
                });

            modelBuilder.Entity("CinemaApp.Data.Models.CinemaMovie", b =>
                {
                    b.Property<Guid>("CinemaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("CinemaId", "MovieId");

                    b.HasIndex("MovieId");

                    b.ToTable("CinemasMovies");
                });

            modelBuilder.Entity("CinemaApp.Data.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Director")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9550f1c9-56a2-4bb7-abbd-c2bc510f826e"),
                            Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO.",
                            Director = "Christopher Nolan",
                            Duration = 148,
                            Genre = "Sci-Fi",
                            ReleaseDate = new DateTime(2010, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Inception"
                        },
                        new
                        {
                            Id = new Guid("9416243e-cd80-4825-9c7e-6e4b2a015e7c"),
                            Description = "When the menace known as The Joker emerges from his mysterious past, he wreaks havoc and chaos on the people of Gotham.",
                            Director = "Christopher Nolan",
                            Duration = 152,
                            Genre = "Action",
                            ReleaseDate = new DateTime(2008, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Dark Knight"
                        });
                });

            modelBuilder.Entity("CinemaApp.Data.Models.CinemaMovie", b =>
                {
                    b.HasOne("CinemaApp.Data.Models.Cinema", "Cinema")
                        .WithMany("CinemaMovies")
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CinemaApp.Data.Models.Movie", "Movie")
                        .WithMany("CinemaMovies")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cinema");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("CinemaApp.Data.Models.Cinema", b =>
                {
                    b.Navigation("CinemaMovies");
                });

            modelBuilder.Entity("CinemaApp.Data.Models.Movie", b =>
                {
                    b.Navigation("CinemaMovies");
                });
#pragma warning restore 612, 618
        }
    }
}
