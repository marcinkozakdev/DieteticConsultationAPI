using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class DietController : ControllerBase
    {
        private const string _prefix = "api";

        private readonly IDietService _dietService;
        public DietController(IDietService dietService)
        {
            _dietService = dietService;
        }

        [Route(_prefix)]
        [HttpPost]
        public ActionResult Post([FromBody] CreateDietDto dto)
        {
            var dietId = _dietService.Create(dto);
            return Created(dietId.ToString(), null);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var diet = await _dietService.GetDiet(id);
            return Ok(diet);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok();
        }
        
        [HttpPut]
        public IActionResult Update()
        {
            return Ok();
        }
        
    }

}