using Webstore.API.Application.Identity.Abstractions;
using Webstore.API.Application.Identity.DTOs;
using Webstore.API.Application.Identity.Models;

namespace Webstore.API.Application.Identity;

public class AuthService : IAuthService
{
    public Task<AuthResult> LoginAsync(LoginRequestDto loginRequest, string? ipAddress = null, string? userAgent = null) => throw new NotImplementedException();
    public Task<AuthResult> RefreshTokenAsync(string refreshToken, string? ipAddress = null, string? userAgent = null) => throw new NotImplementedException();
    public Task<bool> RevokeTokenAsync(string refreshToken, int userId) => throw new NotImplementedException();
}
