using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api/patient/{patientId}/diet")]
    [ApiController]
    public class DietController : ControllerBase
    {
        private readonly IDietService _dietService;

        public DietController(IDietService dietService)
        {
            _dietService = dietService;
        }
        public ActionResult Post([FromRoute]int patientId, [FromBody] CreateDietDto dto)
        {
            var dietId = _dietService.Create(patientId, dto);
            return Created($"api/patient/{patientId}/diet/{dietId}", null);
        }
    }

}
