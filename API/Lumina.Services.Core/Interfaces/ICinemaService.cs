using Lumina.Service.Common.DTO_s.Input;
using Lumina.Service.Common.DTO_s.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Interfaces;

public  interface ICinemaService
{
    Task<int> GetCinemasCount();

    Task<IEnumerable<CinemaOutputResponse>> GetAllAsync();

    Task<CinemaResponse> CreateAsync(CinemaRequest cinemaInput);

    Task<CinemaOutputResponse> GetCinemaByIdAsync(Guid id);

    Task<CinemaResponse> UpdateCinemaAsync(Guid id,CinemaRequest request);

    Task<CinemaResponse> SoftDelete(Guid id);
}
