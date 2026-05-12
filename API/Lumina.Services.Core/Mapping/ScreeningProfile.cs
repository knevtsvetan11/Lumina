using AutoMapper;
using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Input.Screening;
using Lumina.Service.Common.DTO_s.Output.Screening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Mapping;

public class ScreeningProfile : Profile
{

    public ScreeningProfile()
    {
        CreateMap<Screening, ScreeningCreateDto>();
        CreateMap<ScreeningCreateDto, Screening>();
        CreateMap<Screening, ScreeningReadDto>();
        CreateMap<ScreeningUpdateDto, Screening>();
    }
}
