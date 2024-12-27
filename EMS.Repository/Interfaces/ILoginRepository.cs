using EMS.Core.Models;

namespace EMS.Repository.Interfaces;

public interface ILoginRepository
{
    Task<ApiResult> GetByUserNameAsync(string username);
    Task<ApiResult> AddAsync(string username, string passwordHash);
    Task<ApiResult> UpdateRefreshToken(int id, string newToken, DateTime newExpiryDate);
    Task<ApiResult> AddRefreshToken(int userId, string token, DateTime expiryTime);
    Task<ApiResult> GetByRefreshToken(string refreshToken);
    Task<ApiResult> GetUsernameByIdAsync(int userId);
    Task<ApiResult> DeleteRefreshToken(string token);
}
