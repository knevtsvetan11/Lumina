using ApiLumina.Area.Identity.DTO_s;

namespace ApiLumina.Area.Identity.Services.Interfaces;

public interface IManageUserService
{
    Task<IEnumerable<UserOutputDto>> GetAllUsersPaged(UserSearchDto dto);
    
}
