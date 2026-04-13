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
            // check if user already exists
            var existing = _repo.GetByUserName(req.UserName);
            if(existing != null)
            {
                // if password matches, treat as login: issue token and return success
                if (BCrypt.Net.BCrypt.Verify(req.Password,existing.PasswordHash))
                {
                    var token = _userauth.Authenticate(req.UserName, req.Password);
                    if (token == null)
                        return StatusCode(500, new { message = "failed to generate authentication token" });

                    //admin check by username
                    if (existing.UserName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                        return Ok(new { message = " Admin logged in successfully", token });



                    return Ok(new { message = " user logged in  successfully ", token });
                }
                //user exists but wrong password
                return Conflict(new { message = "user aleady exists. this is u r account, Please provdie the correct password for log in " });

            }
                
            // create new user
            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);
            var user = new User
            {
                UserName = req.UserName,
                Email = req.Email,
                PasswordHash = hash,
                CreatedOn = DateTime.UtcNow
            };

            _repo.Create(user);

            var newToken = _userauth.Authenticate(req.UserName, req.Password);
            return Ok(new
            {
                message = "user created successfully",

               user=new { user.Id, user.UserName, user.Email },
               token=newToken
            });
        }


    }
}
