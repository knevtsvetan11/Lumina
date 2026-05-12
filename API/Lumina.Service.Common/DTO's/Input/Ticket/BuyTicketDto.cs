using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Input.Ticket;

public class BuyTicketDto
{
    [Required]
    public Guid ScreeningId { get; set; }

    
    [Required]
    [Range(1,10)]
    public int Tickets { get; set; }
}
