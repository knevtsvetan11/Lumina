using Lumina.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Configuration;

public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> entity)
    {

        entity
            .HasKey(m => m.Id);

    
        entity
            .Property(d => d.IsDeleted)
            .HasDefaultValue(false);

        entity
            .Property(u => u.UserId)
            .IsRequired(true);

        entity
            .HasOne(u => u.User)
            .WithOne(u => u.Manager)
            .HasForeignKey<Manager>(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity
            .HasIndex(m => new { m.UserId })
            .IsUnique();

        entity
            .HasQueryFilter(m => m.IsDeleted == false);
    }
}
