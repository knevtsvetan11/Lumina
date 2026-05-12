using ApiLumina.Area.Identity.DTO_s;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLumina.Area.Identity.Services.Interfaces;

public interface IPermissionsService
{
    Task<IEnumerable<string>> GetAllPermissions();

    Task<bool> UpdateUserPermission(UserPermissionsUpdateDto input);

    Task<IEnumerable<string>> GetRoleClaims(ApplicationUser user);

    Task<IEnumerable<UserPermissionsDto>> GetUserPermissions(ApplicationUser user);

    Task<IEnumerable<string>> GetPermissionsByRole(Guid roleId);

    Task<bool> UpdatePermissions(Guid roleId, List<string> newPermissionsToAssign);
}
