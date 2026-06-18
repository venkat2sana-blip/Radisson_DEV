using Radisson_RHG.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Radisson_RHG.Services;

namespace Radisson_RHG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly IRepositoryUserInterface _repo;
        private readonly IUserAuthServices _userauth;
       public AuthController(IRepositoryUserInterface repo,IUserAuthServices userauth)
        {
            _repo = repo;
            _userauth = userauth;
        }


        public record RegisterRequest(string UserName,string Email ,string Password);

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (_repo.GetByUserName(req.UserName )!= null)
                return Conflict(new { message = "user already exists." });
                
            // create new user
            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);
            var user = new User
            {
                UserName = req.UserName,
                Email = req.Email,
                PasswordHash = hash,
                CreatedOn = DateTime.UtcNow,
                Role="User"
            };

            _repo.Create(user);


            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email
            });
           
            
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] Radisson_RHG.Models.LoginRequest reqs)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var token = _userauth.Authenticate(reqs.Username, reqs.Password);
            if (token == null)
                return Unauthorized(new { message = "invalid username or password" });

            var user = _repo.GetByUserName(reqs.Username);
            var role = user?.Role ?? "user";

            //choose redirect path for frontend

            var redirectUrl = role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                ? "/admin/dashboard"
                : "/user/home";

            return Ok(new { token, role, redirectUrl });
        }


    }
}
