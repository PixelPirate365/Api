using Microsoft.AspNetCore.Mvc;
using web_api.Models;
using web_api.Services;

namespace web_api.Controllers {
    [ApiController]
    //[Route("[controller]")]
    [Route("users")]
    public class PeopleController : ControllerBase {
        private readonly IPeopleService _service;


        public PeopleController(IPeopleService service) {
            _service = service;

        }
        // GET users
        [HttpGet]
        public ActionResult<List<Person>> GetList() {
            return _service.GetAll();
        }

        // GET users/1
        [HttpGet("{id:int:min(1)}")]
        public ActionResult<Person> GetPerson([FromRoute] int id) {
            var ret = _service.GetOne(id);
            if (ret == null)
                return NotFound();
            return ret;
        }

        // POST users
        [HttpPost]
        public ActionResult<Person> AddPerson([FromBody] Person person) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            _service.Add(person);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }
    }
}
