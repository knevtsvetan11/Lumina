

using ApiLumina.Area.Identity.DTO_s;
using ApiLumina.Area.Identity.Services.Interfaces;
using Lumina.Data.Models;
using Lumina.Services.Core.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.Linq.Expressions;
using System.Reflection;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static Lumina.Services.Core.Constants.Permissions;

namespace ApiLumina.Area.Identity.Services;

public class PermissionsService(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager) : IPermissionsService
{
    public async Task<IEnumerable<string>> GetRoleClaims(ApplicationUser user)
    {
        List<string> userRoleClaims = new List<string>();
        var roleNames = await userManager.GetRolesAsync(user);

        foreach (var roleName in roleNames)
        {
            IdentityRole<Guid> role = await roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                var claims = await roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                    userRoleClaims.Add(claim.Value);
            }
        }
        return userRoleClaims;
    }

    public async Task<IEnumerable<UserPermissionsDto>> GetUserPermissions(ApplicationUser user)
    {

        List<string> allPermissions = Permissions.GetValues();

        List<UserPermissionsDto> allClaimsOutputDto = new List<UserPermissionsDto>();

        var roleClaims = await GetRoleClaims(user);

        foreach (var claim in roleClaims)
        {
            var permisssion = new UserPermissionsDto
            {
                PermissionValue = claim,
                IsGranted = true,
                IsInheritedFromRole = true,

            };
            allClaimsOutputDto.Add(permisssion);
        }

        var userClaims = await userManager.GetClaimsAsync(user);

        foreach (var claim in userClaims)
        {
            if (!roleClaims.Contains(claim.Value))
            {
                var permisssion = new UserPermissionsDto
                {
                    PermissionValue = claim.Value,
                    IsGranted = true,
                    IsInheritedFromRole = false,

                };
                allClaimsOutputDto.Add(permisssion);
            }

        }

        foreach (var permission in allPermissions)
        {
            if (!allClaimsOutputDto.Any(p => p.PermissionValue == permission))
            {
                var permisssion = new UserPermissionsDto
                {
                    PermissionValue = permission,
                    IsGranted = false,
                    IsInheritedFromRole = false,
                };
                allClaimsOutputDto.Add(permisssion);
            }
        }
        return allClaimsOutputDto;
    }

    public async Task<IEnumerable<string>> GetAllPermissions()
    {
        List<string> allValidConstants = Permissions.GetValues();
        return allValidConstants;
    }


    public async Task<IEnumerable<string>> GetPermissionsByRole(Guid roleId)
    {
        var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

        var allClaims = await roleManager.GetClaimsAsync(role);

        var allPermissions = allClaims
            .Where(t => t.Type.Contains("Permission"))
            .Select(t => t.Value)
            .ToList();


        return allPermissions;
    }


    public async Task<bool> UpdateUserPermission(UserPermissionsUpdateDto input)
    {
        ApplicationUser user = await userManager.FindByIdAsync(input.UserId.ToString());
        if (user is null)
            return false;

        List<string> allValidConstants = Permissions.GetValues();

        var userClaims = await userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims)
        {
            if (!input.Permissions.Any(p => p == claim.Value))
                await userManager.RemoveClaimAsync(user, claim);
        }

        foreach (var perm in input.Permissions)
        {
            Claim claim = new Claim("Permission", perm.Trim());
            if (allValidConstants.Any(p => p == perm) && !userClaims.Any(c => c.Type is "Permission" && c.Value == perm))
                await userManager.AddClaimAsync(user, claim);
        }
        return true;

    }
    public async Task<bool> UpdatePermissions(Guid roleId, List<string> newPermissionsToAssign)
    {
        IdentityRole<Guid> role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role is null)
            return false;


        List<string> allValidConstants = Permissions.GetValues();

        List<string> sanitizedInput = newPermissionsToAssign
        .Where(p => allValidConstants.Contains(p))
        .ToList();


        IEnumerable<Claim> roleClaims = await roleManager.GetClaimsAsync(role);

        foreach (var claim in roleClaims)
        {
            if (!sanitizedInput.Contains(claim.Value))
                await roleManager.RemoveClaimAsync(role, claim);
        }

        foreach (var perm in sanitizedInput)
        {
            Claim newClaim = new Claim("Permission", perm);
            if (!roleClaims.Any(c => c.Value == newClaim.Value))
                await roleManager.AddClaimAsync(role, newClaim);
        }

        return true;
    }

}
