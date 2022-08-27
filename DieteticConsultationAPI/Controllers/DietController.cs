using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api/diet")]
    [ApiController]
    public class DietController : ControllerBase
    {
        private readonly IDietService _dietService;
        public DietController(IDietService dietService)
        {
            _dietService = dietService;
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateDietDto dto)
        {
            var dietId = _dietService.Create(dto);
            return Created(dietId.ToString(), null);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var diets = _dietService.GetAll();
            return Ok(diets);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    var diet = await _dietService.GetDiet(id);
        //    return Ok();
        //}

        //[HttpDelete]
        //public IActionResult Delete()
        //{
        //    return Ok();
        //}

        //[HttpPut]
        //public IActionResult Update()
        //{
        //    return Ok();
        //}

    }

}