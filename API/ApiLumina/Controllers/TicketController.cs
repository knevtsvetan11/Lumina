using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Input.Ticket;
using Lumina.Service.Common.DTO_s.Output.Ticket;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLumina.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketController : BaseController
{
    private readonly ITicketService _ticketService;
    private readonly IScreeningService _screeningService;
    public TicketController(ITicketService ticketService, IScreeningService screeningService)
    {
        _ticketService = ticketService;
        _screeningService = screeningService;
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()

    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid userIdToGuid;
        if (!Guid.TryParse(userId, out userIdToGuid))
            return Unauthorized("Invalid user ID");

        IEnumerable<ReadTicketDto> userTickets = await this._ticketService.GetAllTicketsAsync(userIdToGuid);
        return Ok(userTickets);
    }


    [HttpPost("buy")]
    public async Task<IActionResult> Buy(BuyTicketDto input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid userIdToGuid;
        if (!Guid.TryParse(userId, out userIdToGuid))
            return BadRequest("Invalid or empty User Id");


        bool success = await this._ticketService.BuyAsync(userIdToGuid, input.ScreeningId, input.Tickets);
        if (!success)
            return BadRequest("All tickets are sell or screening not exist.");

        return Ok(new { success = true, message = "Successly buy ticket." });
    }

}
