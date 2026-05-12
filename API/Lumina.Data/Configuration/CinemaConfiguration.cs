using Lumina.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Configuration;

public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
{
    public void Configure(EntityTypeBuilder<Cinema> entity)
    {

        entity
            .HasKey(c => c.Id);


        entity
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(80);

        entity
            .Property(c => c.Location)
            .IsRequired()
            .HasMaxLength(50);

        entity
             .Property(c => c.IsDeleted)
             .HasDefaultValue(false);

        entity
            .HasOne(m => m.Manager)
            .WithMany(m => m.ManagedCinemas)
            .HasForeignKey(m => m.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        entity
            .HasQueryFilter(c => c.IsDeleted == false);


        entity.HasData(this.SeedCinemas());


    }

    private IEnumerable<Cinema> SeedCinemas()
    {
        IEnumerable<Cinema> cinemas = new List<Cinema>()
        {

            new Cinema
            {
            Id = Guid.Parse("30c8c49e-647d-4a5c-ba64-012263bd0ae5"),
            Name = "Cinema City",
            Location = "Sofia"
            },
            new Cinema
            {
                Id = Guid.Parse("7c7cc780-c803-47df-b307-e901be7b399c"),
                Name = "Arena The Mall",
                Location = "Sofia"
            },
            new Cinema
            {
                Id = Guid.Parse("f244c1c5-e25f-477d-aa01-703d994c6dd1"),
                Name = "Cine Grand Park Center",
                Location = "Plovdiv"
            },
            new Cinema
            {
                Id = Guid.Parse("ed1f1d71-1c10-4c83-8164-a8be57ee4715"),
                Name = "Kino Odeon",
                Location = "Varna"
            },
            new Cinema
            {
                Id = Guid.Parse("0a6b27df-a577-46bf-86ad-1f44562edd20"),
                Name = "Star Cinema",
                Location = "Burgas"
            }
           };
        return cinemas;
    }

}
