using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api/dietician")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DieticianController : ControllerBase
    {
        private readonly IDieticianService _dieticianService;
       
        public DieticianController(IDieticianService dieticianService)
        {
            _dieticianService = dieticianService;
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateDieticianDto dto)
        {
            var id = _dieticianService.CreateDietician(dto);

            return Created($"/api/dietician/{id}", null);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var dieticians = _dieticianService.GetAllDieticians();

            return Ok(dieticians);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var dietician = _dieticianService.GetDietician(id);

            return Ok(dietician);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] UpdateDieticianDto dto, [FromRoute] int id)
        {
           _dieticianService.UpdateDietician(dto, id);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _dieticianService.DeleteDietician(id);

            return NotFound(); 
        }
    }
}
