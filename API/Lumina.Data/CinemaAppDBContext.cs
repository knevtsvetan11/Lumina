namespace Lumina.Data;

using Lumina.Data.Configuration;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
public class CinemaAppDBContext :
     IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{

    public CinemaAppDBContext(DbContextOptions<CinemaAppDBContext> options)
        : base(options)
    {

    }


    public virtual DbSet<Movie> Movies { get; set; } = null!;

    public virtual DbSet<ApplicationUserMovie> ApplicationUserMovies { get; set; } = null!;

    public virtual DbSet<Cinema> Cinemas { get; set; } = null!;

    public virtual DbSet<Screening> Screenings { get; set; } = null!;

    public virtual DbSet<Ticket> Tickets { get; set; } = null!;

    public virtual DbSet<Manager> Managers { get; set; } = null!;

    public virtual DbSet<ChatMessages> ChatMessages { get; set; } = null!;




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new MovieConfiguration());

        modelBuilder.ApplyConfiguration(new ApplicationUserMovieConfiguration());

        modelBuilder.ApplyConfiguration(new CinemaConfiguration());

        modelBuilder.ApplyConfiguration(new ScreeningConfiguration());

        modelBuilder.ApplyConfiguration(new TicketConfiguration());

        modelBuilder.ApplyConfiguration(new ManagerConfiguration());

        modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
    }

}
