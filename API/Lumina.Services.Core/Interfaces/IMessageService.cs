using Lumina.Service.Common.DTO_s.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<MessageResponse>> GetAllActiveMessagesAsync(string adminId);

    Task<IEnumerable<MessageResponse>> GetGroupHistoryAsync(Guid groupId, DateTime? oldestMessageDateTime);

    Task<MessageResponse> SendMessageAsync(MessageRequest request, string senderId);
}
