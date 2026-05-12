using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Models;

public class Screening
{
    public Guid Id { get; set; } 


    public Guid MovieId { get; set; }


    public virtual Movie Movie { get; set; }  

    
    public Guid CinemaId { get; set; } 


    public  virtual Cinema Cinema { get; set; } = null!;


    public int AvailableTickets { get; set; }   


    public bool IsDeleted { get; set; }  

    public string Showtime { get; set; } = null!;

    public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();  

}
