using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<ActionResult> Create([FromBody] DietDto command)
        {
            await _dietService.Create(command);

            return Created("api/diet", null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dietician")]

        public async Task<IActionResult> GetAll()
        {
            var diets = await _dietService.GetAll();

            return Ok(diets);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Get(int id)
        {
            var diet = await _dietService.GetById(id);

            return Ok(diet);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<IActionResult> Update([FromBody] DietDto command)
        {
            await _dietService.Update(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dietService.Delete(id);

            return NoContent();
        }
    }
}