using AutoMapper;
using DieteticConsultationAPI.Entities;
using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult Create([FromBody] CreateDieticianDto dto)
        {
            var id = _dieticianService.CreateDietician(dto);
            return Created($"/api/dietician/{id}", null);
        }

        [HttpGet]
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
           _dieticianService.UpdateDietician(id, dto);
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
