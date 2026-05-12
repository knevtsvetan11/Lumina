using Lumina.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Configuration;

public class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
{
    public void Configure(EntityTypeBuilder<Screening> entity)
    {
        entity
            .HasKey(c => c.Id);

        entity
            .HasOne(c => c.Movie)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.MovieId);


        entity
         .HasOne(c => c.Cinema)
         .WithMany()
         .IsRequired()
         .HasForeignKey(c => c.CinemaId);

        entity
           .Property(c => c.AvailableTickets)
           .HasDefaultValue(0)
           .IsRequired();

        entity
            .HasQueryFilter(c => c.IsDeleted == false);

        entity
            .Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        entity
           .Property(c => c.Showtime)
           .IsRequired()
           .HasMaxLength(5);


        entity
            .HasOne(c => c.Movie)
            .WithMany(c => c.ScreeningProjections)
            .HasForeignKey(c => c.MovieId);

        entity
            .HasOne(c => c.Cinema)
            .WithMany(c => c.ScreeningMovies)
            .HasForeignKey(c => c.CinemaId);

        entity.HasData(ProjectSeeds());
    }

    private IEnumerable<Screening> ProjectSeeds()
    {
        return new Screening[]
        {
            new Screening
            {
                Id = Guid.Parse("8e1b72e6-40c7-4525-bf51-3eb2116e1040"),
                MovieId = Guid.Parse("C9850E58-01C5-4263-C422-08DE3500570B"),
                CinemaId = Guid.Parse("30C8C49E-647D-4A5C-BA64-012263BD0AE5"),
                AvailableTickets = 150,
                IsDeleted = false,
                Showtime = "18:00"
            },new Screening
            {
                Id = Guid.Parse("fd910bcc-d507-413d-8a45-91375537e2a2"),
                MovieId = Guid.Parse("C9850E58-01C5-4263-C422-08DE3500570B"),
                CinemaId = Guid.Parse("30C8C49E-647D-4A5C-BA64-012263BD0AE5"),
                AvailableTickets = 100,
                IsDeleted = false,
                Showtime = "20:00"
            },new Screening
            {
                Id = Guid.Parse("1aa3bdcd-202b-43dc-9979-44a847f37359"),
                MovieId = Guid.Parse("E00208B1-CB12-4BD4-8AC1-36AB62F7B4C9"),
                CinemaId = Guid.Parse("30C8C49E-647D-4A5C-BA64-012263BD0AE5"),
                AvailableTickets = 120,
                IsDeleted = false,
                Showtime = "21:00"
            },
            new Screening
            {
                Id = Guid.Parse("9024bb4f-f351-45d6-9587-53c739435960"),
                MovieId = Guid.Parse("AE50A5AB-9642-466F-B528-3CC61071BB4C"),
                CinemaId = Guid.Parse("30C8C49E-647D-4A5C-BA64-012263BD0AE5"),
                AvailableTickets = 180,
                IsDeleted = false,
                Showtime = "19:30"
            },
            new Screening
            {
                Id = Guid.Parse("c27ca254-597a-4dfd-9d9a-d09d11960769"),
                MovieId = Guid.Parse("54082F99-023B-4D68-89AC-44C00A0958D0"),
                CinemaId = Guid.Parse("30C8C49E-647D-4A5C-BA64-012263BD0AE5"),
                AvailableTickets = 90,
                IsDeleted = false,
                Showtime = "16:45"
            }
        };
    }
}
