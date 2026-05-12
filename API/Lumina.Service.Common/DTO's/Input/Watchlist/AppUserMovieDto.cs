using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Input.Watchlist;

public  class AppUserMovieDto
{
    public Guid UserId { get; set; }

    public Guid MovieId { get; set; }
}
