using ApiLumina.Area.Identity.DTO_sl;
using AutoMapper;
using Lumina.Data.Models;

namespace ApiLumina.Area.Identity.Mapping;

public class AuthProfile : Profile
{

    public AuthProfile()
    {

        CreateMap<RegisterRequest, ApplicationUser>();
    }

}
