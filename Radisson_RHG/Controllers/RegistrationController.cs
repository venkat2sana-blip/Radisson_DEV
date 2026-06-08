using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Radisson_RHG.Services;
using Microsoft.AspNetCore.JsonPatch;
using Radisson_RHG.Models;

namespace Radisson_RHG.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RegistrationController : Controller
    {


        private readonly ILogger<RegistrationController> _logger;
        private readonly IRegistrationInterface _registrationinterface;
        public RegistrationController(ILogger<RegistrationController> logger,IRegistrationInterface registerint)
        {
            _logger = logger;
            _registrationinterface = registerint;

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

        [HttpPost]
        public ActionResult<Registration> Create([FromBody] RegistrationCreateDto rege)
        {
            //if (rege == null)
            //    return BadRequest();
            //var insert = _registrationinterface.Create(rege);
            //return Ok(insert);

            if (rege == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var entity = new Registration
            {
                Name = rege.Name,
                Mobile = rege.Mobile,
                Email = rege.Email,
                Age = rege.Age,
                Gender = !string.IsNullOrEmpty(rege.Gender) ? rege.Gender[0] : '0',
                CreatedOn = rege.CreatedOn ?? DateTime.UtcNow
            };
            var insert = _registrationinterface.Create(entity);
            return Ok(insert);

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
