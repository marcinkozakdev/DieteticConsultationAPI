using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
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
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] DieticianDto command)
        {
            await _dieticianService.Create(command);

            return Created("api/dietician", null);
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var dieticians = await _dieticianService.GetAll();

            return Ok(dieticians);
        }

        [HttpGet("{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var dietician = await _dieticianService.GetById(id);

            return Ok(dietician);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Dietician")]
        public async Task<IActionResult> Update([FromBody] DieticianDto command)
        {
            await _dieticianService.Update(command);

            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _dieticianService.Delete(id);

            return NoContent();
        }
    }
}