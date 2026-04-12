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
            if (_repo.GetByUserName(req.UserName) != null)
                return Conflict(new { message = "user aleady exists" });

            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);
            var user = new User
            {
                UserName = req.UserName,
                Email = req.Email,
                PasswordHash = hash,
                CreatedOn = DateTime.UtcNow
            };

            _repo.Create(user);
            return Ok(new { user.Id, user.UserName, user.Email });
        }


    }
}
