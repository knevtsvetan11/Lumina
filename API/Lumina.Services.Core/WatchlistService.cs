using Lumina.Data.Models;
using Lumina.Data.Repository.Interfaces;
using Lumina.Service.Common.DTO_s.Input.Watchlist;
using Lumina.Service.Common.DTO_s.Output.Wachlist;
using Lumina.Services.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core;

public class WatchlistService : IWatchlistService
{
    private readonly IWatchlistRepository _watchlistRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public WatchlistService(IWatchlistRepository watchlistRepository, UserManager<ApplicationUser> userManager)
    {
        this._watchlistRepository = watchlistRepository;
        this._userManager = userManager;
    }

    public async Task<IEnumerable<UserWatchlistMoviesDto>> GetAllAsync(Guid userId)
    {

        ApplicationUser user = await this._userManager.Users
            .Include(w => w.WatchlistMovies)
            .ThenInclude(m => m.Movie)
            .FirstOrDefaultAsync(u => u.Id == userId);


        IEnumerable<UserWatchlistMoviesDto> appUserMovies = user.WatchlistMovies
            .Select(uw => new UserWatchlistMoviesDto
            {
                MovieId = uw.Movie.Id,
                Title = uw.Movie.Title,
                Genre = uw.Movie.Genre,
                ReleaseData = uw.Movie.ReleaseDate,
                Director = uw.Movie.Director,
                Duration = uw.Movie.Duration,
                Description = uw.Movie.Description,
                ImageUrl = uw.Movie.ImageUrl,
            }).ToList();

        return appUserMovies;
    }

    public async Task<bool> AddAsync(Guid userId, Guid movieId)
    {

        ApplicationUserMovie newAppUserMovie = new ApplicationUserMovie
        {
            ApplicationUserId = userId,
            MovieId = movieId
        };

        bool isExist = await this._watchlistRepository.IsExist(newAppUserMovie);
        if (isExist)
            return false;

        await this._watchlistRepository.AddAsync(newAppUserMovie);
        return true;

    }

    public async Task<bool> DeleteAsync(Guid userId, Guid movieId)
    {
        ApplicationUserMovie appUserMovie = new ApplicationUserMovie
        {
            ApplicationUserId = userId,
            MovieId = movieId
        };

        bool isExist = await this._watchlistRepository.IsExist(appUserMovie);

        if (!isExist)
            return false;
        await this._watchlistRepository.DeleteAsync(appUserMovie);
        return true;

    }

    public async Task<bool> IsExist(Guid movieId, Guid userId)
    {
        ApplicationUserMovie appUserMovie = new ApplicationUserMovie
        {
            ApplicationUserId = userId,
            MovieId = movieId
        };
        bool isMovieExistIUserWatchlist = await this._watchlistRepository.IsExist(appUserMovie);
        if (!isMovieExistIUserWatchlist)
            return false;
        return true;
    }
}
