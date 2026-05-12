using ApiLumina.Hubs;
using Lumina.Service.Common.DTO_s.Input;
using Lumina.Service.Common.DTO_s.Output;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nest;
using System.Threading.Tasks;


namespace ApiLumina.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController(IMovieService _movieService, IHubContext<NotificationHub> hubContext) : BaseController //primary ctor
{


    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(Guid id)
    {
        MovieDetailsDto movie = await _movieService.GetMovieByIdAsync(id);
        if (movie is null)
            return NotFound();
        return Ok(movie);
    }


    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        int moviesCount = await _movieService.MoviesCount();
        return Ok(moviesCount);
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<MovieResponse> allMovies = await _movieService.GetAllMoviesAsync();
        if (allMovies is null)
            return NotFound("Not exist Movies.");
        return this.Ok(allMovies);
    }


    [ProducesResponseType<MovieResponse>(200)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("[action]")]
    [Authorize(Policy = "MovieCreatePolicy")]
    public async Task<IActionResult> Create([FromBody] MovieRequest request)
    {
        MovieResponse result = await _movieService.AddMovieAsync(request);
        if (result is null)
            return StatusCode(409, "Movie with this title already exist.");
        await hubContext.Clients.All.SendAsync("MovieCreated", result);

        return Ok(result);
    }

    [ProducesResponseType<MovieResponse>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPatch("{id}")]
    [Authorize(Policy = "MovieEditPolicy")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MovieRequest request)
    {
        MovieResponse response = await _movieService.UpdateMovieAsync(id, request);
        if (response is null)
            return NotFound("Movie can't edited because not exist.");
        await hubContext.Clients.All.SendAsync("MovieUpdated", response);
        return Ok(response);
    }


    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Policy = "MovieDeletePolicy")]
    public async Task<IActionResult> Delete(Guid id)
    {
        MovieResponse response = await _movieService.SoftDelete(id);
        if (response is null)
            return NotFound("Movie 'Id' not exist or is deleted.");
        await hubContext.Clients.All.SendAsync("MovieDeleted", response);
        return NoContent();
    }



}
