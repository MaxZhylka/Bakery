using backend.Core.DTOs;
using backend.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using backend.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Core.Enums;
using backend.Core.Models;

public class AuthService(IAuthRepository authRepository, ILoggerRepository loggerRepository, IHttpContextAccessor httpContextAccessor) : IAuthService
{
  private readonly IAuthRepository _authRepository = authRepository;
  private readonly ILoggerRepository _loggerRepository = loggerRepository;
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

  public async Task<UserTokensDTO> Login(string email, string password, string? deviceId)
  {
    var user = await _authRepository.GetUserByEmailAsync(email) ?? throw new UnauthorizedAccessException("Користувач не знайдений");
    if (!VerifyPassword(password, user.Password, email))
      throw new UnauthorizedAccessException("Невірний пароль");

    var accessToken = GenerateToken(user.Id.ToString(), user.Email, user.Role);

    var refreshToken = Guid.NewGuid().ToString();


    if (!string.IsNullOrEmpty(deviceId) && await _authRepository.CheckTokenByDeviceIdAsync(deviceId))
    {
      string newDeviceId = Guid.NewGuid().ToString();
      await _authRepository.UpdateRefreshTokenByIdAsync(user.Id, refreshToken, deviceId, newDeviceId);
    }
    else
    {
      deviceId = Guid.NewGuid().ToString();
      await _authRepository.SaveRefreshTokenAsync(user.Id, refreshToken, deviceId);
    }

    var remoteIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;
    var sanitizedIp = remoteIpAddress != null && !remoteIpAddress.IsIPv4MappedToIPv6
        ? remoteIpAddress.ToString()
        : "Invalid IP";
    await _loggerRepository.SaveLogAsync(new UserActionLog
    {
      Id = Guid.NewGuid(),
      UserId = user.Id,
      Operation = Operations.Login,
      Details = $"User '{user.Email}' logged in from device: {deviceId}, IP: {sanitizedIp}",
      Timestamp = DateTime.UtcNow
    });

    return new UserTokensDTO
    {
      Id = user.Id,
      Email = user.Email,
      Name = user.Name,
      Role = user.Role,
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      DeviceId = deviceId
    };
  }

  public async Task<UserTokensDTO> Refresh(string? refreshToken, string? deviceId)
  {
    if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(deviceId))
      throw new UnauthorizedAccessException("refreshToken or deviceId not found");

    UserDTO user = await _authRepository.CheckTokenAsync(refreshToken, deviceId) ??
    throw new UnauthorizedAccessException("Users with this refresh token and device id not found");

    var newRefreshToken = Guid.NewGuid().ToString();
    var newDeviceId = Guid.NewGuid().ToString();
    var newAccessToken = GenerateToken(user.Id.ToString(), user.Email, user.Role);

    await _authRepository.UpdateRefreshTokenAsync(refreshToken, newRefreshToken, newDeviceId);
    return new UserTokensDTO
    {
      Name = user.Name,
      Email = user.Email,
      Role = user.Role,
      RefreshToken = newRefreshToken,
      DeviceId = newDeviceId,
      AccessToken = newAccessToken
    };
  }

  public async Task Logout(string? refreshToken)
  {
    if (string.IsNullOrEmpty(refreshToken))
      throw new UnauthorizedAccessException("Not registered");

    await _authRepository.DeleteRefreshTokenAsync(refreshToken);
  }

  private string GenerateToken(string userId, string email, UserRole role)
  {
    var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role.ToString())
        };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DotNetEnv.Env.GetString("SECRET_KEY")));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "your-app",
        audience: "your-app",
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private static bool VerifyPassword(string password, string hashedPassword, string email)
  {
    var passwordHasher = new PasswordHasher<string>();

    return passwordHasher.VerifyHashedPassword(email, hashedPassword, password) == PasswordVerificationResult.Success;
  }
}
