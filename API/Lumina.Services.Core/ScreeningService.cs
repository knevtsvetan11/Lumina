using AutoMapper;
using Lumina.Data.Models;
using Lumina.Data.Repository.Interfaces;
using Lumina.Service.Common.DTO_s.Input.Screening;
using Lumina.Service.Common.DTO_s.Output.Screening;
using Lumina.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core;

public class ScreeningService(IScreeningRepository _screeningRepository,
        IMovieRepository _movieRepository, ICinemaRepository _cinemaRepository, IMapper _mapper) : IScreeningService
{


    public async Task<int> GetCount()
    {
        return await _screeningRepository.GetAllAttached().CountAsync();
    }
    public async Task<ScreeningShowtimeDto> GetScreeningShowTime(Guid cinemaId, Guid movieId)
    {

        IEnumerable<Screening>? screenings = _screeningRepository
            .GetAllAttached()
            .Where(s => s.CinemaId == cinemaId && s.MovieId == movieId);

        HashSet<string> allShowtimes = screenings.Select(s => s.Showtime).ToHashSet<string>();
        if (!screenings.Any() || allShowtimes.Count == 0)
            return null;

        ScreeningShowtimeDto screeningShowtime = new ScreeningShowtimeDto
        {
            CinemaId = cinemaId,
            MovieId = movieId,
        };
        screeningShowtime.Showtime = allShowtimes;

        return screeningShowtime;
    }
    public async Task<Screening> FirstOrDefaultAsync(Guid id)
    {
        Screening screeningExist = await _screeningRepository.FirstOrDefaultAsync(x => x.Id == id);
        return screeningExist;
    }

    public async Task<ScreeningReadDto> GetScreeningByIdAsync(Guid id)
    {
        Screening existScreening = await _screeningRepository
            .GetAllAttached()
            .Include(s => s.Movie)
            .Include(s => s.Cinema)
            .FirstOrDefaultAsync(s => s.Id == id);

        ScreeningReadDto resultDto = _mapper.Map<ScreeningReadDto>(existScreening);
        return resultDto;
    }

    public async Task<ScreeningReadDto> CreateScreeningAsync(ScreeningCreateDto input)
    {
        Screening? existScreening = await _screeningRepository
            .FirstOrDefaultAsync(s => s.CinemaId == input.CinemaId && s.Showtime == input.Showtime && !s.IsDeleted);

        if (existScreening != null)
            throw new InvalidOperationException("Screening already exist...");
        Movie movieExist = await _movieRepository.GetByIdAsync(input.MovieId);
        Cinema cinemaExist = await _cinemaRepository.GetByIdAsync(input.CinemaId);
        if (movieExist == null)
            throw new KeyNotFoundException($"Movie with ID: {input.MovieId} not found.");
        if (cinemaExist == null)
            throw new KeyNotFoundException($"Cinema with ID: {input.CinemaId} not found.");

        Screening newScreening = _mapper.Map<Screening>(input);

        await _screeningRepository.AddAsync(newScreening);

        ScreeningReadDto outputResult = _mapper.Map<ScreeningReadDto>(newScreening);
        return outputResult;
    }

    public async Task<IEnumerable<ScreeningReadDto>> GetScreeningsByCinemaIdAsync(Guid id)
    {
        IEnumerable<Screening> allScreenings = await _screeningRepository
             .GetAllAttached()
             .Include(s => s.Movie)
             .Include(s => s.Cinema)
             .Where(s => s.CinemaId == id && !s.IsDeleted)
             .ToArrayAsync();

        IEnumerable<ScreeningReadDto> allScreeningsDto = _mapper.Map<IEnumerable<ScreeningReadDto>>(allScreenings);

        return allScreeningsDto;
    }

    public async Task<bool> SoftDeleted(Guid id)
    {
        Screening screeningExist = await _screeningRepository.GetByIdAsync(id);
        if (screeningExist is null or { IsDeleted: true })
            return false;

        screeningExist.IsDeleted = true;
        await _screeningRepository.SaveChangesAsync();
        return true;
    }


    public async Task<ScreeningReadDto> UpdateScreeningAsync(Guid id, ScreeningUpdateDto input)
    {

        Screening? existScreening = await _screeningRepository.GetByIdAsync(id);

        if (existScreening is null)
            throw new KeyNotFoundException("Screening not exist.");
        if (existScreening.IsDeleted)
            throw new KeyNotFoundException("Screening is deleted.");
        Movie? movieExist = await _movieRepository.GetByIdAsync(input.MovieId);
        if (movieExist is null || movieExist.IsDeleted)
            throw new KeyNotFoundException("Movie with this 'ID' not exist or is deleted.");
        Cinema? cinemaExist = await _cinemaRepository.GetByIdAsync(input.CinemaId);

        if (cinemaExist is null or { IsDeleted: true })
            throw new KeyNotFoundException("Cinema with this 'ID' not exist or is deleted.");

        Screening? existDublicateScreening = await _screeningRepository
           .GetAllAttached()
           .FirstOrDefaultAsync(s => s.CinemaId == input.CinemaId && s.Showtime == input.Showtime
           && !s.IsDeleted && s.Id != id);
        if (existDublicateScreening is not null)
            throw new InvalidOperationException("Updated screening have conflict with another screen.");

        _mapper.Map(input, existScreening);
        await _screeningRepository.SaveChangesAsync();

        ScreeningReadDto result = _mapper.Map<ScreeningReadDto>(existScreening);
        return result;
    }

}
