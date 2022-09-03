using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public ActionResult Create([FromBody] CreatePatientDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var patientId = _patientService.CreatePatient(dto);

            return Created(patientId.ToString(), null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dietician")]
        public IActionResult GetAll([FromQuery]PatientQuery query)
        {
            var patients = _patientService.GetAllPatients(query);

            return Ok(patients);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public IActionResult Get(int id)
        {
            var patient = _patientService.GetPatient(id);

            return Ok(patient);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public IActionResult Update([FromBody] UpdatePatientDto dto, int id)
        {
            _patientService.UpdatePatient(dto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public IActionResult Delete(int id)
        {
            _patientService.DeletePatient(id);

            return NoContent();
        }
    }
}

