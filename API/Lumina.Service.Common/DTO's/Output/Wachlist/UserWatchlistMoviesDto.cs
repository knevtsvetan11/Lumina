using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Output.Wachlist;

public record class UserWatchlistMoviesDto
{

    public Guid MovieId { get; init; }

    public string Title { get; init; }

    public string Genre { get; init; }

    public DateTime ReleaseData { get; init; }

    public string Director { get; init; }

    public int Duration { get; init; }

    public string Description { get; init; }

    public string? ImageUrl { get; init; }

    public bool IsDeleted { get; init; }

}
