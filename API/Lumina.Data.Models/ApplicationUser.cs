
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Models;

public  class ApplicationUser : IdentityUser<Guid>
{
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public  Manager? Manager { get; set; }

    public virtual ICollection<ApplicationUserMovie> WatchlistMovies { get; set; }
        = new HashSet<ApplicationUserMovie>();

    public virtual ICollection<Ticket> Tickets { get; set; }
       = new HashSet<Ticket>();

}
