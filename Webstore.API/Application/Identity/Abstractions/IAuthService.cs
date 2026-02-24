using Webstore.API.Application.Identity.DTOs;

namespace Webstore.API.Application.Identity.Abstractions;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequestDto loginRequest, string? ipAddress = null, string? userAgent = null);
    Task<AuthResult> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null);
    Task<bool> RevokeTokenAsync(string refreshToken, int userId);
}