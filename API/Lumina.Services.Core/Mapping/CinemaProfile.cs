using AutoMapper;
using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Input;
using Lumina.Service.Common.DTO_s.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Mapping;

public class CinemaProfile : Profile
{
    public CinemaProfile()
    {
        CreateMap<Cinema, CinemaOutputResponse>();
        CreateMap<CinemaRequest,Cinema>();
        CreateMap<Cinema, CinemaResponse>();
        CreateMap<Cinema, CinemaResponse>();
        CreateMap<CinemaRequest,Cinema>();
    }
}
