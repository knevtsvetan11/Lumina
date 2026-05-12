using ApiLumina.Area.Identity.DTO_s;
using ApiLumina.Area.Identity.Services.Interfaces;
using ApiLumina.Controllers;
using Lumina.Data.Models;
using Lumina.Services.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using static Lumina.Services.Core.Constants.Permissions;

namespace ApiLumina.Area.Identity.Controllers;

[Route("api/permissions")]
[Authorize(Roles = "Admin")]
public class PermissionsController(RoleManager<IdentityRole<Guid>> roleManager, IPermissionsService permissionsService, UserManager<ApplicationUser> userManager) : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var allPermissions = await permissionsService.GetAllPermissions();
        return Ok(allPermissions);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserPermissions(Guid id)
    {
        ApplicationUser user = userManager.Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            return NotFound();
        var userPermissions = await permissionsService.GetUserPermissions(user);
        return Ok(userPermissions);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetPermissionsByRoleId(Guid id)
    {
        var roleExist = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (roleExist is null)
            return NotFound("Role not exist");
        var allPermission = await permissionsService.GetPermissionsByRole(id);
        return Ok(allPermission);
    }


    [HttpPatch("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserPermissions([FromBody] UserPermissionsUpdateDto input)
    {
        bool result = await permissionsService.UpdateUserPermission(input);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpPatch("{roleId}")]
    public async Task<IActionResult> UpdatePermissions(Guid roleId, [FromBody] List<string> newPermissions)
    {

        bool result = await permissionsService.UpdatePermissions(roleId, newPermissions);
        if (!result)
            return NotFound();

        return NoContent();

    }

}
