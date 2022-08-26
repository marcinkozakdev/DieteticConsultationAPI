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
            _patientService.Update(dto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _patientService.Delete(id);
            return NotFound();
        }

        [HttpPost]
        public ActionResult Add([FromBody] AddPatientDto dto)
        {
            var id = _patientService.Add(dto);
            return Created($"/api/patient/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<PatientDto>> GetAll()
        {
            var patients = _patientService.GetAll();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public ActionResult<PatientDto> Get([FromRoute] int id)
        {
            var patient = _patientService.GetById(id);
            return Ok(patient);
        }

    }


}

