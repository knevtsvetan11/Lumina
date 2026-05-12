using ApiLumina.Area.Identity.DTO_s;
using ApiLumina.Area.Identity.DTO_s.Login;
using ApiLumina.Area.Identity.DTO_sl;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiLumina.Area.Identity.Services.Interfaces;

public interface IAuthService
{

    Task<IdentityResult> RegisterAsync(RegisterRequest input);

    Task<LoginResponse> LoginAsync(LoginRequest request);

    string GetUserId(ClaimsPrincipal user);
}
