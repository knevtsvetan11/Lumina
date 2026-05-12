using ApiLumina.Area.Identity.DTO_s;
using ApiLumina.Area.Identity.Services.Interfaces;
using ApiLumina.Controllers;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace ApiLumina.Area.Identity.Controllers;

[Route("api/users")]
public class ManageUsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> _roleManager, IManageUserService userService) : BaseController
{

    [Authorize(Policy = "UserViewPolicy")]
    [ProducesResponseType(typeof(IEnumerable<UserOutputDto>), StatusCodes.Status200OK)]
    [HttpGet("paged")]
    public async Task<IActionResult> GetUsersPaged([FromQuery] UserSearchDto input)
    {
        IEnumerable<UserOutputDto> usersPaged = await userService.GetAllUsersPaged(input);
        return Ok(usersPaged);
    }


    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        int usersCount = await userManager.Users.CountAsync();
        return Ok(usersCount);
    }

    [Authorize(Policy = "UserDeletePolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (String.IsNullOrWhiteSpace(id))
            return BadRequest("User's 'Id' is null or incorrect.");
        ApplicationUser user = await userManager.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not exist.");
        if (user.IsDeleted is true)
            return BadRequest("User is already deleted.");

        user.IsDeleted = true;
        await userManager.UpdateAsync(user);
        return Ok(new { userDeleted = true });
    }


    [Authorize(Policy = "UserEditPolicy")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> ToggleIsActiveUser(string id)
    {
        ApplicationUser user = await userManager.FindByIdAsync(id);
        if (user is null)
            return NotFound("User not exist.");
        user.IsActive = !user.IsActive;

        await userManager.UpdateAsync(user);
        return Ok(new { isUserActive = user.IsActive });

    }


    [Authorize(Policy = "UserViewPolicy")]
    [HttpGet("{email}")]
    public async Task<IActionResult> CheckEmailExist(string email)
    {
        ApplicationUser existUserWithThisEmail = await userManager.FindByEmailAsync(email);
        if (existUserWithThisEmail is not null)
            return Ok(new { exist = true });

        return Ok(new { exist = false });

    }


    [Authorize(Policy = "CreateUserPolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto input)
    {

        ApplicationUser newUser = await userManager.FindByEmailAsync(input.Email);
        if (newUser is not null)
            return Conflict(new { massage = $"User with this email:{input.Email} already exist." });
        if (input.Role is "Admin")
            return Forbid();
        var role = await _roleManager.FindByNameAsync(input.Role);
        if (role is null)
            return BadRequest(new { massage = "Role not exist" });

        newUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Email = input.Email,
            UserName = input.Email
        };

        var createdResult = await userManager.CreateAsync(newUser, input.Password);
        if (!createdResult.Succeeded)
            return BadRequest(new { massage = "Error to create user" });
        var createRoleResult = await userManager.AddToRoleAsync(newUser, input.Role);
        if (!createdResult.Succeeded)
            return BadRequest(new { massage = "Error to assign role to user." });

        return Ok(new { message = $"Successfully created new user." });
    }


    [HttpPatch]
    public async Task<IActionResult> EditUser(UserRequest input)
    {
        return NoContent();
    }


}
