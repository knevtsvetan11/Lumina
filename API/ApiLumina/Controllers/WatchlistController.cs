using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Input.Watchlist;
using Lumina.Service.Common.DTO_s.Output.Wachlist;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLumina.Controllers;

[ApiController]
[Route("api/watchlists")]
public class WatchlistController : BaseController
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService)
    {
        this._watchlistService = watchlistService;

    }


    [HttpGet("[action]")]
    public async Task<IActionResult> ShowWatchlist()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid userIdToGuid;
        if (!Guid.TryParse(userId, out userIdToGuid))
            return NotFound(new { errorMassage = "The user with ID is not found.", succes = false });
        IEnumerable<UserWatchlistMoviesDto> appUserMovies = await this._watchlistService.GetAllAsync(userIdToGuid);
        return Ok(appUserMovies);
    }

    [HttpPost("{movieId}")]
    public async Task<IActionResult> Add(Guid movieId)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        Guid userIdtoGuid;
        if (!Guid.TryParse(userId, out userIdtoGuid))
            return BadRequest(new { message = "User Id is not valid" });
        if (movieId == Guid.Empty)
            return BadRequest(new { message = "Movie Id is not valid" });

        bool isAdded = await this._watchlistService.AddAsync(userIdtoGuid, movieId);
        if (isAdded == false)
            return Conflict(new { message = "Movie already exist in watchlist" });

        return Ok(new
        {
            inWatchlist = true,
            message = "Succussly added to watchlist."
        });
    }

    [HttpDelete("{movieId}")]
    public async Task<IActionResult> Delete(Guid movieId)
    {

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid userIdtoGuid;
        if (!Guid.TryParse(userId, out userIdtoGuid))
            return BadRequest(new { errorMessage = "The user with ID is not found." });
        if (movieId == Guid.Empty)
            return BadRequest(new { errorMessage = "Movie's 'Id' cannot be empty." });


        bool isDeleted = await this._watchlistService.DeleteAsync(userIdtoGuid, movieId);
        if (isDeleted == false)
            return NotFound(new { errorMessage = "Movie not exist in watchlist ", isDeleted = false });

        return Ok(new { isDeleted = true });
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> IsMovieExistInWatchlist(Guid movieId)
    {
        if (movieId == Guid.Empty)
            return NotFound(new { massage = "Movie with Id: not exist." });

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid userIdtoGuid;
        if (!Guid.TryParse(userId, out userIdtoGuid))
            return NotFound(new { massage = "User with Id not exist." });

        bool IsMovieExistInWatchlist = await this._watchlistService.IsExist(movieId, userIdtoGuid);
        if (!IsMovieExistInWatchlist)
            return Ok(new { isExist = false });
        return Ok(new { isExist = true });


    }

}
