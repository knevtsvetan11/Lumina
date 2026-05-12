using AutoMapper;
using Lumina.Data.Models;
using Lumina.Data.Repository.Interfaces;
using Lumina.Service.Common.DTO_s.Input;
using Lumina.Service.Common.DTO_s.Output;
using Lumina.Services.Core.Interfaces;
using Microsoft.Identity.Client;
using Nest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core;

public class CinemaService(ICinemaRepository cinemaRepository, IMapper mapper) : ICinemaService
{


    public async Task<int> GetCinemasCount()
    {
        return cinemaRepository.GetAllAttached().Count();
    }

    public async Task<IEnumerable<CinemaOutputResponse>> GetAllAsync()
    {
        IEnumerable<Cinema> allCinemas = await cinemaRepository
            .GetAllAsync();
        IEnumerable<CinemaOutputResponse> allCinemasDto = mapper
            .Map<IEnumerable<CinemaOutputResponse>>(allCinemas);
        return allCinemasDto;
    }

    public async Task<CinemaResponse> CreateAsync(CinemaRequest request)
    {
        Cinema? cinema = await cinemaRepository
            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower());

        if (cinema != null)
            return null;
        cinema = new Cinema();
        mapper.Map(request, cinema);
        await cinemaRepository.AddAsync(cinema);

        CinemaResponse response = new CinemaResponse();
        mapper.Map(cinema, response);
        return response;
    }

    public async Task<CinemaOutputResponse> GetCinemaByIdAsync(Guid id)
    {
        Cinema? cinema = await cinemaRepository.GetByIdAsync(id);
        if (cinema is null)
            return null;

        CinemaOutputResponse cinemaResponse = mapper.Map<CinemaOutputResponse>(cinema);
        return cinemaResponse;
    }

    public async Task<CinemaResponse> UpdateCinemaAsync(Guid id, CinemaRequest request)
    {
        Cinema? cinema = await cinemaRepository.FirstOrDefaultAsync(c => c.Id == id && c.Name != request.Name);

        if (cinema is null)
            return null;

        mapper.Map(request, cinema);
        await cinemaRepository.SaveChangesAsync();

        CinemaResponse response = new CinemaResponse();
        mapper.Map(cinema, response);

        return response;
    }

    public async Task<CinemaResponse> SoftDelete(Guid id)
    {
        Cinema? cinema = await cinemaRepository.GetByIdAsync(id);
        if (cinema is null || cinema.IsDeleted == true)
            return null;

        cinema.IsDeleted = true;
        await cinemaRepository.SaveChangesAsync();
        CinemaResponse response = new CinemaResponse();
        mapper.Map(cinema, response);
        return response;
    }

}
