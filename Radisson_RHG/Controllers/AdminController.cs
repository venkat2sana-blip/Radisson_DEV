using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Radisson_RHG.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        [HttpGet("dashboard")]
        public IActionResult Dashboard() => Ok(new { message ="welcome to admin dashboard"});
       
    }
}
