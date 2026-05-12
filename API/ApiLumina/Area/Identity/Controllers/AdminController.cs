using ApiLumina.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLumina.Area.Identity.Controllers;


[Authorize(Roles ="Admin")]
public class AdminController : BaseController
{
    
}
