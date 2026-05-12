using Lumina.Data.Models;
using Lumina.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Repository;

public class ScreeningRepository : GenericRepository<Screening, Guid>, IScreeningRepository
{
    public ScreeningRepository(CinemaAppDBContext cinemaDbContext) : base(cinemaDbContext)
    {
    }
}
