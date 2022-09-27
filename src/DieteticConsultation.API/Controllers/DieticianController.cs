using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api/dietician")]
    [ApiController]
    public class DieticianController : ControllerBase
    {
        private readonly IDieticianService _dieticianService;
       
        public DieticianController(IDieticianService dieticianService)
        {
            _dieticianService = dieticianService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [AllowAnonymous]
        public IActionResult Get([FromRoute] int id)
        {
            var dietician = _dieticianService.GetDietician(id);

            return Ok(dietician);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietician")]
        public IActionResult Update([FromBody] UpdateDieticianDto dto, [FromRoute] int id)
        {
           _dieticianService.UpdateDietician(dto, id);

            return Ok();
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            _dieticianService.DeleteDietician(id);

            return NoContent(); 
        }
    }
}
