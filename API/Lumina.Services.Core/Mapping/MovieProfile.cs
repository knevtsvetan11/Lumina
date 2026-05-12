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

public class MovieProfile : Profile
{
    public MovieProfile()
    {

        CreateMap<Movie, MovieResponse>();

        CreateMap<MovieRequest, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<Movie, MovieResponse>();

        CreateMap<MovieRequest, Movie>();
        CreateMap<Movie, MovieResponse>()
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<Movie, MovieDetailsDto>();
    }

}
