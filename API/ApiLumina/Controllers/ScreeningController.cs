using Lumina.Service.Common.DTO_s.Input.Screening;
using Lumina.Service.Common.DTO_s.Output.Screening;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace ApiLumina.Controllers;

[ApiController]
[Route("api/screenings")]
public class ScreeningController(IScreeningService _screenService) : BaseController
{


    [HttpGet]
    public async Task<IActionResult> GetCount()
    {
        int screeningsCount = await _screenService.GetCount();
        return Ok(screeningsCount);

    }


    [Authorize(Policy = "ScreeningViewPolicy")]
    [HttpGet("{cinemaId}/{movieId}")]
    public async Task<IActionResult> GetShowtimes(Guid cinemaId, Guid movieId)
    {
        if (cinemaId == Guid.Empty || movieId == Guid.Empty)
            return NotFound();
        ScreeningShowtimeDto showtimes = await _screenService.GetScreeningShowTime(cinemaId, movieId);
        if (showtimes == null)
            return NotFound(new { messages = "Screening not found or not available showtimes." });
        return Ok(showtimes);


    }


    [AllowAnonymous]
    [HttpGet("cinema/{id}/screenings")]
    public async Task<IActionResult> GetScreeningsByCinema(Guid id)
    {
        IEnumerable<ScreeningReadDto> allScreenings = await _screenService.GetScreeningsByCinemaIdAsync(id);
        return Ok(allScreenings);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetScreeningById(Guid id)
    {
        ScreeningReadDto result = await _screenService.GetScreeningByIdAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }


    [Authorize(Policy = "ScreeningCreatePolicy")]
    [HttpPost]
    public async Task<IActionResult> Create(ScreeningCreateDto input)
    {
        ScreeningReadDto resultCreate = await _screenService.CreateScreeningAsync(input);
        if (resultCreate is null)
            return Conflict();

        return CreatedAtAction(
        nameof(GetScreeningById),
        new { id = resultCreate.Id }, resultCreate.Id);

    }


    [Authorize("ScreeningDeletePolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool isDeleted = await _screenService.SoftDeleted(id);
        if (!isDeleted)
            return NotFound("Screening is not exist or is already deleted.");
        return Ok();
    }


    [Authorize(Policy = "ScreeningEditPolicy")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, ScreeningUpdateDto input)
    {

        ScreeningReadDto result = await _screenService.UpdateScreeningAsync(id, input);
        return Ok(result);
    }

}
