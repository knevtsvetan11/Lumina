using ApiLumina.Area.Identity.DTO_s;
using ApiLumina.Area.Identity.DTO_s.Login;
using ApiLumina.Area.Identity.DTO_sl;
using ApiLumina.Area.Identity.Services.Interfaces;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ApiLumina.Area.Identity.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole<Guid>> roleManager) : Controller
{

   

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterRequest input)
    {
        var result = await authService.RegisterAsync(input);
        if (result.Succeeded)
            return Ok(new  { message="User is successly registered." });

            return BadRequest($"User with {input.Email} already exist.");

    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        LoginResponse response = await authService.LoginAsync(request); 
        
        if (response is null)
            return Unauthorized("Invalid login.");

        return Ok(response);
    }


}
