using ApiLumina.Area.Identity.DTO_s;
using ApiLumina.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiLumina.Area.Identity.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/role/manage")]
public class RoleManageController(RoleManager<IdentityRole<Guid>> roleManager) : BaseController
{


    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await roleManager.Roles
        .Select(r => new ReadRolesDto
        {
            Id = r.Id,
            Name = r.Name
        }).ToListAsync();

        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleDto input)
    {

        bool existRole = await roleManager.RoleExistsAsync(input.RoleName);
        if (existRole)
            return Conflict("Role is already exist.");
        IdentityRole<Guid> newRole = new IdentityRole<Guid>(input.RoleName);

        IdentityResult result = await roleManager.CreateAsync(newRole);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        RoleResponseDto response = new RoleResponseDto { Id = newRole.Id, Name = newRole.Name };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {

        IdentityRole<Guid> existRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
        if (existRole is null)
            return NotFound();
        IdentityResult result = await roleManager.DeleteAsync(existRole);
        if (result.Succeeded)
            return NoContent();
        else
            return BadRequest();
    }
}
