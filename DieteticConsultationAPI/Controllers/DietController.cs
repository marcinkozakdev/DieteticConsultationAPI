﻿using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<ActionResult> Create([FromBody] CreateDietDto dto)
        {
            var diet = await _dietService.CreateDiet(dto);

            return Created(diet.ToString(), null);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Dietician")]

        public async Task<IActionResult> GetAll()
        {
            var diets = await _dietService.GetAllDiets();

            return Ok(diets);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Dietician,Patient")]
        public async Task<IActionResult> Get(int id)
        {
            var diet = await _dietService.GetDiet(id);

            return Ok(diet);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<IActionResult> Update([FromBody] UpdateDietDto dto, int id)
        {
            await _dietService.UpdateDiet(dto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Dietician")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dietService.DeleteDiet(id);

            return NoContent();
        }
    }
}