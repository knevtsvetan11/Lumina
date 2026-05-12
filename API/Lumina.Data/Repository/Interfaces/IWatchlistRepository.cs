using Lumina.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Repository.Interfaces;

public  interface IWatchlistRepository
{
    Task AddAsync(ApplicationUserMovie appUserMovie);

    Task DeleteAsync(ApplicationUserMovie appUserMovie);

    IQueryable<ApplicationUserMovie> GetAllAtached();

    Task<bool> IsExist(ApplicationUserMovie input);
}
