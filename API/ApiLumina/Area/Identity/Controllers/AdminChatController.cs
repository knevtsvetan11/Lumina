using ApiLumina.Controllers;
using Lumina.Service.Common.DTO_s.Message;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLumina.Area.Identity.Controllers;

[Route("api/admin/chats")]
[ApiController]
public class AdminChatController(IMessageService messageService) : BaseController
{


    [HttpGet("active/messages")]
    public async Task<IActionResult> GetAllActiveMessages()
    {
        string adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        IEnumerable<MessageResponse> rsponse = await messageService.GetAllActiveMessagesAsync(adminId);
        return Ok(rsponse);

    }

}
