using AutoMapper;
using Lumina.Data.Models;
using Lumina.Data.Repository.Interfaces;
using Lumina.Service.Common.DTO_s.Input;
using Lumina.Service.Common.DTO_s.Output;
using Lumina.Services.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core;

public class MovieService(IMovieRepository movieRepository, IMapper mapper) : IMovieService
{

    public async Task<int> MoviesCount()
    {
        int moviesCount = movieRepository
            .GetAllAttached()
            .Count();
        return moviesCount;
    }

    public async Task<MovieDetailsDto> GetMovieByIdAsync(Guid id)
    {
        Movie? movieExist = await movieRepository.GetByIdAsync(id);
        if (movieExist is null)
            return null;
        MovieDetailsDto movieDetails = mapper.Map<MovieDetailsDto>(movieExist);
        return movieDetails;
    }

    public async Task<IEnumerable<MovieResponse>> GetAllMoviesAsync()
    {

        IEnumerable<Movie> movies = await movieRepository.GetAllAsync();
        IEnumerable<MovieResponse> moviesDto = mapper.Map<IEnumerable<MovieResponse>>(movies);
        return moviesDto;
    }

    public async Task<MovieResponse> AddMovieAsync(MovieRequest inputModel)
    {
        Movie? movie = await movieRepository
            .FirstOrDefaultAsync(m => m.Title.ToLower() == inputModel.Title.ToLower());
        if (movie != null)
            return null;

        Movie movieCreate = mapper.Map<Movie>(inputModel);

        try
        {
            await movieRepository.AddAsync(movieCreate);
        }
        catch (Exception ex)
        {

            throw new Exception("Failed to create and save Movie.", ex);
        }
        MovieResponse output = mapper.Map<MovieResponse>(movieCreate);
        return output;
    }



    public async Task<MovieResponse> UpdateMovieAsync(Guid id, MovieRequest request)
    {
        Movie? movie = await movieRepository
           .GetByIdAsync(id);

        if (movie is null || movie.IsDeleted)
            return null;

        mapper.Map(request, movie);
        await movieRepository.SaveChangesAsync();

        MovieResponse response = new MovieResponse();
        mapper.Map(movie, response);
        return response;
    }

    public async Task<MovieResponse> SoftDelete(Guid id)
    {
        Movie? movie = await movieRepository
            .GetByIdAsync(id);
        if (movie is null || movie.IsDeleted)
            return null;

        movie.IsDeleted = true;
        await movieRepository.SaveChangesAsync();
        MovieResponse response = new MovieResponse();
        mapper.Map(movie, response);

        return response;
    }
}
