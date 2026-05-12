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

public class ApplicationUserMovieConfiguration : IEntityTypeConfiguration<ApplicationUserMovie>
{
    public void Configure(EntityTypeBuilder<ApplicationUserMovie> entity)
    {
        entity.HasKey(aum => new { aum.ApplicationUserId, aum.MovieId });

        entity.Property(aum => aum.ApplicationUserId)
            .IsRequired();

        entity.HasOne(d => d.ApplicationUser)
            .WithMany(d => d.WatchlistMovies)
            .HasForeignKey(d => d.ApplicationUserId);
            
        entity.HasOne(aum => aum.Movie) 
            .WithMany(m=>m.UserWatchlists)
            .HasForeignKey(aum  => aum.MovieId);
    }
}
