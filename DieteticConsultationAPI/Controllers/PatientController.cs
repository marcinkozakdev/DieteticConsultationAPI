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

        [HttpPost]
        public ActionResult Create([FromBody] CreatePatientDto dto)
        {
            var patientId = _patientService.CreatePatient(dto);
            return Created(patientId.ToString(), null);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var patients = _patientService.GetAllPatients();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var patient = _patientService.GetPatient(id);
            return Ok(patient);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] UpdatePatientDto dto, int id)
        {
            _patientService.UpdatePatient(dto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _patientService.DeletePatient(id);
            return NotFound();
        }
    }
}

