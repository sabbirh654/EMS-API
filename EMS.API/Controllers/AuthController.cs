using EMS.Core.DTOs;
using EMS.Core.Helpers;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region commented
        //private readonly ITokenService _tokenService;
        //private readonly ILoginService _loginService;

        //public AuthController(ILoginService loginservice, ITokenService tokenService)
        //{
        //    _tokenService = tokenService;
        //    _loginService = loginservice;
        //}

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] Login request)
        //{
        //    // Validate user (mocked for demo purposes)
        //    if (request.UserName == "admin" && request.Password == "password")
        //    {
        //        var roles = new List<string> { "Admin", "User" };
        //        var token = _tokenService.GenerateToken(request.UserName, roles);
        //        return Ok(new { Token = token });
        //    }

        //    return Unauthorized();
        //}

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register([FromBody] RegisterOrLoginDto dto)
        //{
        //    var result = await _loginService.RegisterUserAsync(dto);
        //    return Ok(result);
        //}
        #endregion

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
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
        {
            var res = _loginService.DeleteRefreshToken(logoutDto); 
            return Ok(res.Result);
        }
    }
}
