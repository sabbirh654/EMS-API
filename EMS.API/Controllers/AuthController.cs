using EMS.Core.Entities;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Validate user (mocked for demo purposes)
            if (request.Username == "admin" && request.Password == "password")
            {
                var roles = new List<string> { "Admin", "User" };
                var token = _tokenService.GenerateToken(request.Username, roles);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
