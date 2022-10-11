using DieteticConsultationAPI.Models;
using DieteticConsultationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DieteticConsultationAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _accountService.RegisterUser(dto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            string token = await _accountService.GenerateJwt(dto);

            return Ok(token);
        }

    }
}
