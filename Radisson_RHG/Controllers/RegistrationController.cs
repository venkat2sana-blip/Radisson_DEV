using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Radisson_RHG.Services;
using Microsoft.AspNetCore.JsonPatch;

namespace Radisson_RHG.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RegistrationController : Controller
    {


        private readonly ILogger<RegistrationController> _logger;
        private readonly IRegistrationInterface _registrationinterface;
        private readonly IRepositoryUserInterface _userrepo;
        private readonly ApplicationDbContext _db;
        public RegistrationController(
        ILogger<RegistrationController> logger,
        IRegistrationInterface registerint,
        IRepositoryUserInterface userRepo,
        ApplicationDbContext db )
        {
            _logger = logger;
            _registrationinterface = registerint;
            _userrepo = userRepo;
            _db = db;

        }


        [HttpGet]
        public ActionResult<IEnumerable<Registration>> Getall() => Ok(_registrationinterface.Getall());

        [HttpGet("by-id/{id}")]
        public ActionResult<Registration> Getbyid(int id)
        {
            var res = _registrationinterface.Getbyid(id);
            if(res==null)
            {
              return  NotFound();

            }
            return Ok(res);
        }

        //create : insert into users and registration tables atomically
        [HttpPost]
        public ActionResult<Registration> Create([FromBody] Registration rege)
        {
            if (rege == null)
                return BadRequest();
<<<<<<< Updated upstream
            var insert = _registrationinterface.Create(rege);
            return Ok(insert);
=======
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // basic duplicate check by mobile or email
            var exists = _db.Users.Any(u => u.UserName == rege.Mobile || u.Email == rege.Email);
            if (exists)
                return Conflict(new { message = "A user with the same mobible or email already exists. please login " });

            // map registraction entity
            var entity = new Registration
            {
                Name = rege.Name,
                Mobile = rege.Mobile,
                Email = rege.Email,
                Age = rege.Age,
                Gender = !string.IsNullOrEmpty(rege.Gender) ? rege.Gender[0] : '0',
                CreatedOn = rege.CreatedOn ?? DateTime.UtcNow
            };

            // begin transaction to ensure both inserts succeed or none

            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // create user (use mobile as username)
                var user = new User
                {
                    UserName=rege.Mobile,
                    Email=rege.Email,
                    PasswordHash=BCrypt.Net.BCrypt.HashPassword(rege.Password),
                    CreatedOn=DateTime.UtcNow,
                    Role="User"
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                _db.registrations.Add(entity);
                _db.SaveChanges();

                transaction.Commit();
                return Ok(entity);
            }
            catch(Exception e)
            {
                try { transaction.Rollback(); } catch{/* ignore rollback errors */ }
                _logger.LogError(e, "Failed to create user registration");
                return StatusCode(500, new { message = "Registration failed" });
            }


            //var insert = _registrationinterface.Create(entity);
            //return Ok(insert);
>>>>>>> Stashed changes

        }

        [HttpPut("{id}")]
        public ActionResult<Registration> Modify(int id,[FromBody] Registration regest)
        {
            var update = _registrationinterface.Modify(id, regest);
            return Ok(update);

            if (update == null)
                return BadRequest();

        }


        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<Registration> patchDoc)
        {
            if (patchDoc == null)
                return NotFound();
            var recordex = _registrationinterface.Getbyid(id);
            if (recordex == null)
                return NotFound();
            patchDoc.ApplyTo(recordex,ModelState);
            if (!TryValidateModel(recordex))
                return ValidationProblem(ModelState);

            _registrationinterface.Modify(id, recordex);
            return Ok(recordex);
        }



        [HttpDelete("{id}")]
        public ActionResult Remove(int id)
        {
            if (!_registrationinterface.Remove(id))
                return BadRequest();
            return NoContent();
        }


        [HttpGet("between")]
       public ActionResult<IEnumerable<Registration>> GetByDateRange(DateTime from,DateTime to)
        {
            if (from == default || to == default)
                return BadRequest("Both from and to query parameters are required in ISO date format,");
            if (from > to)
                return BadRequest("from must be less than or equal to to");
            var fromUtc = from;
            var toUtc = to.Date.AddDays(1).AddTicks(-1);

            var list = _registrationinterface.GetByDateRange(fromUtc, toUtc);
            return Ok(list);

        }


        [HttpGet("by-mobile-or-email")]
        public ActionResult<Registration> GetByMobileOrEmail([FromQuery]string? mobile,[FromQuery]string? email)
        {

            if (string.IsNullOrEmpty(mobile) && string.IsNullOrEmpty(email))
                return BadRequest("please provide atleast mobile or email");
            var returnresult = _registrationinterface.GetByMobileOrEmail(mobile,email);
            if (returnresult == null)
            
                return NotFound("No registration found with the provided details");
            

            return Ok(returnresult);

            
        }
    }
}
