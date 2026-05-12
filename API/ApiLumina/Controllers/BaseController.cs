using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLumina.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
public class BaseController : ControllerBase
{
    
}
