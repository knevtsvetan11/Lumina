using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Models;

public class ChatMessages
{
    public Guid Id { get; set; }

    public virtual ApplicationUser Sender { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public string Message { get; set; }

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

    public Guid ChatGroupId  { get; set; }
}
