using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Output;

public  class MovieDetailsDto
{
    public Guid Id { get; init; }

    public string Title { get; init; }

    public string Genre { get; init; }

    public DateTime ReleaseDate { get; init; }

    public string Director { get; init; }

    public int Duration { get; init; }

    public string Description { get; init; }

    public string? ImageUrl { get; init; }
}
