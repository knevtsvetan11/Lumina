using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Input.Screening;

public  class ScreeningShowtimeDto
{
    public Guid CinemaId { get; set; } 

    public Guid MovieId { get; set; }

    public IEnumerable<string> Showtime { get; set; } = new List<string>();

}
