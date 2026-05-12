using Lumina.Service.Common.DTO_s.Input;
using Lumina.Service.Common.DTO_s.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Interfaces;

public  interface IMovieService
{
    Task<int> MoviesCount();

    Task<MovieDetailsDto> GetMovieByIdAsync(Guid id);

    Task<IEnumerable<MovieResponse>> GetAllMoviesAsync();

    Task<MovieResponse> AddMovieAsync(MovieRequest inputModel);

    Task<MovieResponse> UpdateMovieAsync(Guid id,MovieRequest inputModel);

    Task<MovieResponse> SoftDelete(Guid id);

}
