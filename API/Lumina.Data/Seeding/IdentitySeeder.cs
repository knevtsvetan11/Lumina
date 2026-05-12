using Lumina.Data.Seeding.Interfaces;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Seeding;

public class IdentitySeeder : IIdentitySeeder
{
    private string[] DefaultRoles = { "Admin", "User" };

    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration; 

    public IdentitySeeder(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        this._roleManager = roleManager;
        this._userManager = userManager;
        this._configuration  = configuration;
    }   

    public async Task SeedIdentityAsync()
    {
        await this.SeedRolesAsync();
        await this.SeedUsersAsync();

    }

    private async Task SeedRolesAsync()
    {
      
        foreach (var defRole in DefaultRoles)
        {

            bool roleExist =await  this._roleManager
               .RoleExistsAsync(defRole);

            if (!roleExist)
            {
                IdentityRole newRole = new IdentityRole(defRole);
                IdentityResult result = await this._roleManager
                    .CreateAsync(newRole);
                if (!result.Succeeded)
                {
                    throw new Exception($"There was an exeption while seeding the {defRole} role");
                }
            }
        }
    }

    private async Task SeedUsersAsync()
    {

        string? testUserEmail = this._configuration["UserSeed:TestUser:Email"];
        string? testUserPassword = this._configuration["UserSeed:TestUser:Password"];
        string? adminUserEmail = this._configuration["UserSeed:TestAdmin:Email"];
        string? adminUserPassword = this._configuration["UserSeed:TestAdmin:Password"];

        if(testUserEmail == null|| testUserPassword ==null||
            adminUserEmail==null|| adminUserPassword == null)
        {
            throw new Exception($"There was an exeption while obtaining the {nameof(testUserEmail)},{nameof(testUserPassword)},{nameof(adminUserEmail)},{nameof(adminUserPassword)}");
        }

        ApplicationUser testUser = new ApplicationUser();
        ApplicationUser testUserSeeded = await this._userManager.FindByEmailAsync(testUserEmail);
        if (testUserSeeded == null)
        {
            await this._userManager.SetUserNameAsync(testUser, testUserEmail);
            await this._userManager.SetEmailAsync(testUser, testUserEmail);
            IdentityResult result = await this._userManager.CreateAsync(testUser, testUserPassword);
            if (!result.Succeeded)
            {
                throw new Exception($"There was an exeption while seeding the {testUserEmail} user");
            }

            result = await this._userManager.AddToRoleAsync(testUser, "User");
            if (!result.Succeeded)
            {
                throw new Exception($"There was an exeption while assigning the User role to the {testUserEmail} ");

            }
        }
        ApplicationUser adminUser = new ApplicationUser();

        ApplicationUser? adminUserSeeded = await this._userManager.FindByEmailAsync(adminUserEmail);
        if (adminUserSeeded == null)
        {
            await this._userManager.SetUserNameAsync(adminUser, adminUserEmail);
            await this._userManager.SetEmailAsync(adminUser, adminUserEmail);
            IdentityResult result = await this._userManager.CreateAsync(adminUser, adminUserPassword);
            if (!result.Succeeded)
            {
                throw new Exception($"There was an exeption while seeding the {adminUserEmail} admin");
            }
            result = await this._userManager.AddToRoleAsync(adminUser, "Admin");
            if (!result.Succeeded)
            {
                throw new Exception($"There was an exeption while assigning the Admin role to the{adminUserEmail} ");

            }

        }
    }


}
