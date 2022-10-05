using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Models.Pagination;
using DieteticConsultationAPI.Services.Interfaces;
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
        public async Task<ActionResult> Create([FromBody] CreatePatientDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var patientId = await _patientService.CreatePatient(dto);

            return Created(patientId.ToString(), null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<IActionResult> GetAll([FromQuery]PatientQuery query)
        {
            var patients = await _patientService.GetAllPatients(query);

            return Ok(patients);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _patientService.GetPatient(id);

            return Ok(patient);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Update([FromBody] UpdatePatientDto dto, int id)
        {
            await _patientService.UpdatePatient(dto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Delete(int id)
        {
            await _patientService.DeletePatient(id);

            return NoContent();
        }
    }
}

