using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api/diet")]
    [ApiController]
    public class DietController : ControllerBase
    {
        private readonly IDietService _dietService;

        public DietController(IDietService dietService)
        {
            _dietService = dietService;
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateDietDto dto)
        {
            var dietId = _dietService.CreateDiet(dto);
            return Created(dietId.ToString(), null);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var diets = _dietService.GetAllDiets();
            return Ok(diets);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var diet = _dietService.GetDiet(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] UpdateDietDto dto, int id)
        {
            _dietService.UpdateDiet(dto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _dietService.DeleteDiet(id);
            return NotFound();
        }
    }
}