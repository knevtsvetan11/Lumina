using Lumina.Data;
using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Message;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core;

public class MessageService(CinemaAppDBContext dBContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager) : IMessageService // for antoher services you use repository layer here you not have.....
{

    public async Task<IEnumerable<MessageResponse>> GetAllActiveMessagesAsync(string adminId)
    {
        var adminGuid = Guid.Parse(adminId);

        var response = await dBContext.Users
            .AsNoTracking()
            .Where(u => dBContext.ChatMessages.Any(m => m.SenderId == u.Id && m.ReceiverId == adminGuid))
            .Select(u => dBContext.ChatMessages
                .Where(m => m.SenderId == u.Id && m.ReceiverId == adminGuid)
                .OrderByDescending(m => m.SentAt)
                .Select(m => new MessageResponse
                {
                    Id = m.Id,
                    Message = m.Message,
                    SentAt = m.SentAt,
                    SenderId = u.Id,
                    SenderUsername = u.UserName,
                    SenderFirstName = u.FirstName,
                    SenderLastName = u.LastName,
                    ReceiverId = adminGuid,
                    ChatGroupId = m.ChatGroupId,
                    IsRead = m.IsRead
                })
                .FirstOrDefault()
            )
            .OrderByDescending(x => x.SentAt)
            .ToListAsync();

        return response;

    }

    public async Task<IEnumerable<MessageResponse>> GetGroupHistoryAsync(Guid groupId, DateTime? oldestMessageDateTime)
    {
        var query = dBContext.ChatMessages.AsQueryable();

        if (oldestMessageDateTime is null)
            query = query.Where(x => x.ChatGroupId == groupId);
        else
            query = query.Where(x => x.ChatGroupId == groupId && x.SentAt < oldestMessageDateTime);

        var result = await query
              .OrderByDescending(x => x.SentAt)
          .Take(20)
          .Select(x => new MessageResponse
          {
              Id = x.Id,
              SenderId = x.SenderId,
              ReceiverId = x.ReceiverId,
              Message = x.Message,
              SentAt = x.SentAt,
              IsRead = x.IsRead,
              ChatGroupId = x.ChatGroupId

          })
          .OrderBy(x => x.SentAt)
          .ToListAsync();

        return result;
    }

    public async Task<MessageResponse> SendMessageAsync(MessageRequest request, string senderId)
    {
        Guid receiverId = Guid.Empty;
        ChatMessages message = new ChatMessages();

        if (request.GroupId == "SUPPORT_TEAM")
        {
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            receiverId = admins[0].Id;
            message.ChatGroupId = Guid.Parse(senderId);
        }
        else
        {
            var receiver = await userManager.FindByIdAsync(request.GroupId);
            if (receiver is null)
                return null;
            receiverId = Guid.Parse(request.GroupId);
            message.ChatGroupId = Guid.Parse(request.GroupId);
        }

        message.Id = Guid.NewGuid();
        message.SenderId = Guid.Parse(senderId);
        message.ReceiverId = receiverId;
        message.Message = request.Text;
        message.SentAt = DateTime.Now;
        message.IsRead = false;

        await dBContext.ChatMessages.AddAsync(message);
        await dBContext.SaveChangesAsync();

        MessageResponse response = new MessageResponse
        {
            Id = message.Id,
            SenderId = message.SenderId,
            ReceiverId = message.ReceiverId,
            Message = message.Message,
            SentAt = message.SentAt,
            IsRead = message.IsRead,
            ChatGroupId = message.ChatGroupId
        };
        return response;
    }

}
