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
        public RegistrationController(ILogger<RegistrationController> logger,IRegistrationInterface registerint)
        {
            _logger = logger;
            _registrationinterface = registerint;

        }


        [HttpGet]
        public ActionResult<IEnumerable<Registration>> Getall() => Ok(_registrationinterface.Getall());

        [HttpGet("{id}")]
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
        public ActionResult<Registration> Create([FromBody] Registration rege)
        {
            if (rege == null)
                return BadRequest();
            var insert = _registrationinterface.Create(rege);
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
    }
}
