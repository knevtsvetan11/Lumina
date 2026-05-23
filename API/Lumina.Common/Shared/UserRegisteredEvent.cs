using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Common.Shared;

public record UserRegisteredEvent
{
    public Guid UserId { get; init; }
    public string Email { get; init; }

    public string  Username { get; set; }
    public DateTime RegisteredAt { get; init; }
}