using ApiLumina.Hubs;
using Lumina.Service.Common.DTO_s.Input;
using Lumina.Service.Common.DTO_s.Output;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ApiLumina.Controllers
{
    [ApiController]
    [Route("api/cinemas")]
    public class CinemaController(ICinemaService _cinemaService, IHubContext<NotificationHub> hubContext) : BaseController
    {


        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            int cinemasCount = await _cinemaService.GetCinemasCount();
            return Ok(cinemasCount);
        }



        [AllowAnonymous]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<CinemaOutputResponse> allCinemas = await _cinemaService.GetAllAsync();
            return Ok(allCinemas);
        }


        [ProducesResponseType(202)]
        [ProducesResponseType(409)]
        [Authorize(Policy = "CreateCinemaPolicy")]
        [HttpPost]
        public async Task<IActionResult> Create(CinemaRequest request)
        {
            CinemaResponse response = await _cinemaService.CreateAsync(request);
            if (response is null)
                return Conflict("Cinema with this name already exist.");

            hubContext.Clients.All.SendAsync("CinemaCreated", response);
            return Created();
        }



        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCinemaById(Guid id)
        {
            CinemaOutputResponse cinemaResponse = await _cinemaService.GetCinemaByIdAsync(id);
            if (cinemaResponse is null)
                return NotFound("Cinema with 'Id'cannot be found.");

            return Ok(cinemaResponse);
        }


        [ProducesResponseType<CinemaResponse>(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EditCinemaPolicy")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CinemaRequest request)
        {
            CinemaResponse response = await _cinemaService.UpdateCinemaAsync(id, request);
            if (response is null)
                return NotFound("Cinema with this 'Id' not exist.");
            hubContext.Clients.All.SendAsync("CinemaUpdated", response);
            return Ok(response);

        }


        [Authorize(Policy = "CinemaDeletePolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            CinemaResponse response = await _cinemaService.SoftDelete(id);
            if (response is null)
                return NotFound("Cinema with this 'Id' not exist or is deleted.");
            hubContext.Clients.All.SendAsync("CinemaDeleted", response);
            return NoContent();
        }

    }
}
