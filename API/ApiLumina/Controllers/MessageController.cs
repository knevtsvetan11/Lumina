using ApiLumina.Hubs;
using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Message;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nest;
using System.Security.Claims;

namespace ApiLumina.Controllers;


[Route("api/messages")]
[ApiController]
public class MessageController(IHubContext<SupportChatHub> supportChatHub, IMessageService messageService, UserManager<ApplicationUser> userManager) : BaseController
{




    [ProducesResponseType<IEnumerable<MessageResponse>>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("history/{groupId}/{oldestMessageDateTime?}")]
    public async Task<IActionResult> GetChatHistory(Guid groupId, DateTime? oldestMessageDateTime)
    {
        IEnumerable<MessageResponse> response = await messageService.GetGroupHistoryAsync(groupId, oldestMessageDateTime);
        if (response is null)
            return NotFound();
        return Ok(response);
    }


    [HttpPost("message")]
    public async Task<IActionResult> SendMessage(MessageRequest request)
    {
        var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        MessageResponse response = await messageService.SendMessageAsync(request, senderId);
        if (response is null)
            return NotFound("Receiver not exist");

        await supportChatHub.Clients.Groups(response.ChatGroupId.ToString()
              ).SendAsync("ReceiveMessage", response);

        return Ok();

    }

}
