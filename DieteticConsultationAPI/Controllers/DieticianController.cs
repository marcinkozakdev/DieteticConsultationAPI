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

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateDieticianDto dto, [FromRoute] int id)
        {
           _dieticianService.Update(id, dto);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _dieticianService.Delete(id);
            return NotFound(); 
        }

        [HttpPost]
        public ActionResult Add([FromBody] AddDieticianDto dto)
        {
            var id = _dieticianService.Add(dto);
            return Created($"/api/dietician/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DieticianDto>> GetAll()
        {
            var dieticians = _dieticianService.GetAll();
            return Ok(dieticians);
        }

        [HttpGet("{id}")]
        public ActionResult<DieticianDto> Get([FromRoute] int id)
        {
            var dietician = _dieticianService.GetById(id);
            return Ok(dietician);
        }

    }
}
