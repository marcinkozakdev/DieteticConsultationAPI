using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {

        private readonly IPatientService _patientService;


        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdatePatientDto dto, [FromRoute] int id)
        {
            _patientService.Update(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _patientService.Delete(id);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Add([FromBody] AddDieticianDto dto)
        {
            var id = _patientService.Add(dto);
            return Created($"/api/dietician/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DieticianDto>> GetAll()
        {
            var dieticians = _patientService.GetAll();
            return Ok(dieticians);
        }

        [HttpGet("{id}")]
        public ActionResult<DieticianDto> Get([FromRoute] int id)
        {
            var dietician = _patientService.GetById(id);
            return Ok(dietician);
        }

    }


}

