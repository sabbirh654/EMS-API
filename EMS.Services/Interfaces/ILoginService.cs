using EMS.Core.DTOs;
using EMS.Core.Models;

namespace EMS.Services.Interfaces;

public interface ILoginService
{
    Task<ApiResult> RegisterUserAsync(RegisterDto dto);
    Task<ApiResult> LoginUserAsync(LoginDto dto);
    Task<ApiResult> RefreshUserAsync(RefreshTokenDto dto);
    Task<ApiResult> DeleteRefreshToken(LogoutDto dto);
}
