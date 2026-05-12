using Lumina.Service.Common.DTO_s.Input.Watchlist;
using Lumina.Service.Common.DTO_s.Output.Wachlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Interfaces;

public interface IWatchlistService
{

    Task<IEnumerable<UserWatchlistMoviesDto>> GetAllAsync(Guid userId);

    Task<bool> AddAsync(Guid userId,Guid movieId);

    Task<bool> DeleteAsync(Guid userId,Guid movieId);

    Task<bool> IsExist(Guid movieId, Guid userId);
}
