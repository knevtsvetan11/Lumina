using Lumina.Data.Models;
using Lumina.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Repository;

public class WatchlistRepository : IWatchlistRepository
{
    private readonly CinemaAppDBContext _dbContext;
    private readonly DbSet<ApplicationUserMovie> _appUserMovies;
    public WatchlistRepository(CinemaAppDBContext cinemaAppDBContext)
    {
        this._dbContext = cinemaAppDBContext;
        this._appUserMovies = _dbContext.Set<ApplicationUserMovie>();
    }


    public async Task AddAsync(ApplicationUserMovie appUserMovie)
    {
        await this._appUserMovies.AddAsync(appUserMovie);
        await this._dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ApplicationUserMovie appUserMovie)
    {
        this._appUserMovies.Remove(appUserMovie);
        await this._dbContext.SaveChangesAsync();
    }

    public IQueryable<ApplicationUserMovie> GetAllAtached()
    {
        return this._appUserMovies.AsQueryable();
    }

    public async Task<bool> IsExist(ApplicationUserMovie input)
    {
        return await this._appUserMovies
            .AnyAsync(aum => aum.ApplicationUserId == input.ApplicationUserId && aum.MovieId == input.MovieId);

    }
}
