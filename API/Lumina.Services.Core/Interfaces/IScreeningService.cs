using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Input.Screening;
using Lumina.Service.Common.DTO_s.Output.Screening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Interfaces;

public interface IScreeningService
{
    Task<int> GetCount();

    Task<ScreeningShowtimeDto> GetScreeningShowTime(Guid cinemaId, Guid movieId);

    Task<Screening> FirstOrDefaultAsync(Guid id);

    Task<ScreeningReadDto> GetScreeningByIdAsync(Guid id);

    Task<ScreeningReadDto> CreateScreeningAsync(ScreeningCreateDto input);

    Task<IEnumerable<ScreeningReadDto>> GetScreeningsByCinemaIdAsync(Guid id);

    Task<bool> SoftDeleted(Guid id);

    Task<ScreeningReadDto> UpdateScreeningAsync(Guid id, ScreeningUpdateDto input);

}
