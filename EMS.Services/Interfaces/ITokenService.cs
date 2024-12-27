namespace EMS.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(string username, IList<string> roles = null);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiry();
}
