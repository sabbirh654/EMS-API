using DnsClient.Internal;
using EMS.Core.DTOs;
using EMS.Core.Helpers;
using EMS.Core.Models;
using EMS.Repository.Interfaces;
using EMS.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EMS.Services.Implementations;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;
    private readonly ILogger<LoginService> _logger;
    private readonly ITokenService _tokenService;

    public LoginService(ILoginRepository loginRepository, ILogger<LoginService> logger, ITokenService tokenService)
    {
        _loginRepository = loginRepository;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task<ApiResult> DeleteRefreshToken(LogoutDto dto)
    {
        try
        {
            var res = await _loginRepository.DeleteRefreshToken(dto.RefreshToken!);
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(LoginService), nameof(RegisterUserAsync), ex.Message));
            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.REGISTER_USER_ERROR);
        }
    }

    public async Task<ApiResult> LoginUserAsync(LoginDto dto)
    {
        try
        {
            var existingUser = await _loginRepository.GetByUserNameAsync(dto.Username);

            if (existingUser.Result == null || !BCrypt.Net.BCrypt.Verify(dto.Password, existingUser.Result!.PasswordHash))
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.UNAUTHORIZED_ERROR, ErrorMessage.WRONG_USER_NAME_OR_PASSWORD);
            }

            var accessToken = _tokenService.GenerateToken(existingUser.Result!.Username);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var res = _loginRepository.AddRefreshToken(existingUser.Result.Id, refreshToken, _tokenService.GetRefreshTokenExpiry());

            dynamic result = new
            {
                Token = accessToken,
                RefreshToken = refreshToken,
            };

            return ApiResultFactory.CreateSuccessResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(LoginService), nameof(RegisterUserAsync), ex.Message));
            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.REGISTER_USER_ERROR);
        }
    }

    public async Task<ApiResult> RefreshUserAsync(RefreshTokenDto dto)
    {
        try
        {
            var res = await _loginRepository.GetByRefreshToken(dto.RefreshToken!);

            if (res.Result == null || res.Result!.ExpiresAt < DateTime.UtcNow)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.UNAUTHORIZED_ERROR, "Invalid or expired session");
            }

            var user = await _loginRepository.GetUsernameByIdAsync(res.Result!.UserId);


            var newAccessToken = _tokenService.GenerateToken(user.Result);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var res2 = await _loginRepository.UpdateRefreshToken(res.Result.Id, newRefreshToken, _tokenService.GetRefreshTokenExpiry());

            dynamic result = new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
            };

            return ApiResultFactory.CreateSuccessResult(result);
        }
        catch (Exception ex)
        {
            return ApiResultFactory.CreateErrorResult(ErrorCode.UNAUTHORIZED_ERROR, "Invalid or expired session");
        }
    }

    public async Task<ApiResult> RegisterUserAsync(RegisterDto dto)
    {
        try
        {
            var existingUser = await _loginRepository.GetByUserNameAsync(dto.Username);

            if (existingUser.Result != null)
            {
                return ApiResultFactory.CreateErrorResult(ErrorCode.ALREADY_EXISTS_ERROR, ErrorMessage.USER_ALREADY_EXISTS);
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var res = await _loginRepository.AddAsync(dto.Username, passwordHash);

            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ErrorMessage.GetErrorMessage(nameof(LoginService), nameof(RegisterUserAsync), ex.Message));
            return ApiResultFactory.CreateErrorResult(ErrorCode.INTERNAL_SERVER_ERROR, ErrorMessage.REGISTER_USER_ERROR);
        }
    }
}
