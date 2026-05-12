using ApiLumina.Area.Identity.Configuration;
using ApiLumina.Area.Identity.DTO_s;
using ApiLumina.Area.Identity.DTO_s.Login;
using ApiLumina.Area.Identity.DTO_sl;
using ApiLumina.Area.Identity.Services.Interfaces;
using AutoMapper;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Lumina.Services.Core.Constants.Permissions;

namespace ApiLumina.Area.Identity.Services;

public class AuthService : IAuthService
{
    private UserManager<ApplicationUser> _userManager;
    private IMapper _mapper;
    private readonly JwtSettings _jwtSettings;
    private RoleManager<IdentityRole<Guid>> _roleManager;
    private IHttpContextAccessor _accessor;
    public AuthService(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager,
        IMapper mapper, IOptions<JwtSettings> jwtSettings, IHttpContextAccessor accessor)
    {
        this._userManager = userManager;
        this._mapper = mapper;
        this._jwtSettings = jwtSettings.Value;
        this._roleManager = roleManager;
        this._accessor = accessor;
    }

    public string GetUserId()
    {
        return _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string GetUserId(ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }



    public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await this._userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "User with this email already exists."
            });
        }

        ApplicationUser createUser = this._mapper.Map<ApplicationUser>(request);
        createUser.Id = Guid.NewGuid();

        var result = await this._userManager.CreateAsync(createUser, request.Password);


        await _userManager.AddToRoleAsync(createUser, "User");


        return result;

    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        ApplicationUser userExist = await this._userManager.FindByEmailAsync(request.Email);
        if (userExist is null)
            return null;

        bool result = await this._userManager.CheckPasswordAsync(userExist, request.Password);
        if (!result)
            return null;

        var roles = await this._userManager.GetRolesAsync(userExist);
        var authClaims = new List<Claim>
        {
                 new Claim(ClaimTypes.NameIdentifier, userExist.Id.ToString()),
                 new Claim(ClaimTypes.Email, userExist.Email),
                 new Claim(ClaimTypes.Name, userExist.UserName),

        };

        var userClaims = await this._userManager.GetClaimsAsync(userExist);
        foreach (var claim in userClaims)
            authClaims.Add(claim);

        foreach (var role in roles)
        {
            var roleExist = await _roleManager.FindByNameAsync(role);
            if (roleExist != null)
            {
                var roleClaims = await this._roleManager.GetClaimsAsync(roleExist);
                foreach (var claim in roleClaims)
                    authClaims.Add(claim);
            }
            authClaims.Add(new Claim(ClaimTypes.Role, role));

        }




        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.Expiryminutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        LoginResponse response = new LoginResponse()
        {
            UserId = userExist.Id,
            Username = userExist.UserName,
            UserEmail = userExist.Email,
            UserRole = roles[0],
            Exspiration = DateTime.UtcNow.AddMinutes(120),
            Token = tokenHandler.WriteToken(token)
        };

        return response;
    }

}
