using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Output;

public record class CinemaOutputResponse
{
    [Required]
    public Guid Id { get; init; }

    [Required]
    public string Name { get; init; } = null!; 

    [Required]
    public string Location { get; init; } = null!;

}
