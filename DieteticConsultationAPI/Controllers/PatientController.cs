using DieteticConsultationAPI.Entities;
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

        public PatientController(IPatientService patientService) =>
            _patientService = patientService;


        [HttpPost]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<ActionResult> Create([FromBody] PatientDto command)
        {
            await _patientService.Create(command);

            return Created("api/patient", null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<IActionResult> GetAll([FromQuery] PatientQuery query)
        {
            var patients = await _patientService.GetAll(query);

            return Ok(patients);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _patientService.GetById(id);

            return Ok(patient);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Update([FromBody] PatientDto command)
        {
            await _patientService.Update(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Delete(int id)
        {
            await _patientService.Delete(id);

            return NoContent();
        }
    }
}

