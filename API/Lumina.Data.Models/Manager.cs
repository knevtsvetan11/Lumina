using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Models;

public  class Manager
{

    public Guid Id { get; set; }

    public bool IsDeleted { get; set; } 

    public Guid UserId { get; set; } 

    public virtual ApplicationUser User { get; set; } = null!;

    public virtual ICollection<Cinema> ManagedCinemas { get; set; } = new HashSet<Cinema>();
}
