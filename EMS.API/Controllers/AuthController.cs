using EMS.Core.DTOs;
using EMS.Core.Helpers;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILoginService loginService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var res = await _loginService.RegisterUserAsync(registerDto);

            if (res.IsSuccess == false)
            {
                if (res.ErrorCode == ErrorCode.ALREADY_EXISTS_ERROR)
                {
                    return Conflict(res);
                }
            }

            return Ok(res);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var res = await _loginService.LoginUserAsync(loginDto);

            if (!res.IsSuccess)
            {
                if (res.ErrorCode == ErrorCode.UNAUTHORIZED_ERROR)
                {
                    Unauthorized("Invalid username or password.");
                }
            }

            return Ok(res);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto tokenDto)
        {
            var res = await _loginService.RefreshUserAsync(tokenDto);

            return Ok(res);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
        {
            var res = await _loginService.DeleteRefreshToken(logoutDto);
            return Ok(res.Result);
        }
    }
}
