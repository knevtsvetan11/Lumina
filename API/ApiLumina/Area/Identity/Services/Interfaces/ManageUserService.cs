using ApiLumina.Area.Identity.DTO_s;
using Lumina.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace ApiLumina.Area.Identity.Services.Interfaces;

public class ManageUserService(UserManager<ApplicationUser> userManager) : IManageUserService
{
    public async Task<IEnumerable<UserOutputDto>> GetAllUsersPaged(UserSearchDto input)
    {
        var query = userManager.Users.AsQueryable();

        if (!String.IsNullOrWhiteSpace(input.SearchData))
        {
            string searchData = input.SearchData.ToLower();
            if(input.FilterColumn is null || input.FilterColumn == "all")
            {
                query = query.Where(u => u.UserName.ToLower().Contains(searchData) ||
                     u.Email.ToLower().Contains(searchData) ||
                     u.FirstName.ToLower().Contains(searchData));
                     
            }
            else if(input.FilterColumn is not null)
            {
                switch (input.FilterColumn)
                {
                    case "username":
                        query = query.Where(u => u.UserName.ToLower().Contains(searchData)); break;
                    case "email":
                        query = query.Where(u => u.Email.ToLower().Contains(searchData)); break;
                    case "firstname":
                        query = query.Where(u => u.FirstName.ToLower().Contains(searchData)); break;
                    case "lastname":
                        query = query.Where(u => u.LastName.ToLower().Contains(searchData)); break;
                    case "addres":
                        query = query.Where(u => u.Address.ToLower().Contains(searchData)); break;
                    case "phone":
                        query = query.Where(u => u.PhoneNumber.ToLower().Contains(searchData)); break;
                }
            }
            
        }

        query = query.OrderBy(u => u.Email);
        query = query.Skip(input.PageIndex * input.PageSize).Take(input.PageSize);

        IEnumerable<ApplicationUser> result = await query.ToListAsync();
        int allUsersInDb =await userManager.Users.CountAsync();
        IEnumerable<UserOutputDto> resultMapDto = result
            .Select(u => new UserOutputDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.PhoneNumber,
                Addres = u.Address,
                CountUsers = allUsersInDb

            }).ToList();


      return resultMapDto;
    }
}
