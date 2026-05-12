using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumina.Data.Repository.Interfaces;
using Lumina.Data.Models;
namespace Lumina.Data.Repository.Interfaces;

public  interface ICinemaRepository : IGenericRepository<Cinema, Guid>
{
}
