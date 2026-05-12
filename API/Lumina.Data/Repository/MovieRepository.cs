using Lumina.Data.Models;
using Lumina.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Repository;

public class MovieRepository : GenericRepository<Movie, Guid>, IMovieRepository
{
    public MovieRepository(CinemaAppDBContext cinemaDbContext) : base(cinemaDbContext)
    {
    }
}
