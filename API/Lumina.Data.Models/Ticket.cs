using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Models;

public  class Ticket
{

    public Guid Id { get; set; }

    public decimal Price { get; set; }

    public Guid? ScreeningId { get; set; }

    public Screening? ScreeningProjection{ get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public int Count { get; set; } 
}
