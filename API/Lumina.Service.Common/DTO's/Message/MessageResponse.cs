using Lumina.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Message;

public class MessageResponse
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }

    public string SenderUsername { get; set; }

    public string SenderFirstName { get; set; }

    public string SenderLastName { get; set; }

    public Guid ReceiverId { get; set; }

    public string Message { get; set; }

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

    public Guid ChatGroupId { get; set; }
}
