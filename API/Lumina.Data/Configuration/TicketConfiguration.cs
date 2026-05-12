using Lumina.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Configuration;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{

    public void Configure(EntityTypeBuilder<Ticket> entity)
    {
        entity
           .HasKey(t => t.Id);


        entity
            .Property(t => t.Price)
             .HasPrecision(18, 2)
            .IsRequired();

        entity
            .HasOne(c => c.ScreeningProjection)
            .WithMany(c => c.Tickets)
            .HasForeignKey(c => c.ScreeningId);
          

        entity
            .Property(t => t.UserId)
            .IsRequired();

        entity
            .HasOne(t => t.User)
            .WithMany(t =>t.Tickets)
            .HasForeignKey(t => t.UserId)
            .IsRequired(true);

    }

}
