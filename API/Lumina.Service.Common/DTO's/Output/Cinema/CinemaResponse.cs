using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Output;

public record class CinemaResponse
{
    public Guid Id { get; init; } 

    public string Name { get; init; }

    public string Location { get; init; }
}
