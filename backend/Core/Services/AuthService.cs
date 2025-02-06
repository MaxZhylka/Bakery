using backend.Core.DTOs;
using backend.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.Services;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using System.Security.Claims;
using backend.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backend.Core.Models;

public class AuthService : IAuthService
{
  private readonly IAuthRepository _authRepository;

  public AuthService( IAuthRepository authRepository)
  {
    _authRepository = authRepository;
  }

  public async Task<UserTokensDTO> Login(string email, string password, string? deviceId)
  {
    var user = await _authRepository.GetUserByEmailAsync(email);
    if (user == null)
      throw new UnauthorizedAccessException("Користувач не знайдений");

    if (!VerifyPassword(password, user.Password))
      throw new UnauthorizedAccessException("Невірний пароль");

    var accessToken = GenerateToken(user.Id.ToString(), user.Email, user.Role);

    var refreshToken = Guid.NewGuid().ToString();


    if (!string.IsNullOrEmpty(deviceId) && await _authRepository.CheckTokenByDeviceIdAsync(deviceId))
    {
      string newDeviceId = Guid.NewGuid().ToString();
      await _authRepository.UpdateRefreshTokenByIdAsync(user.Id, refreshToken, deviceId, newDeviceId);
    } else {
      deviceId = Guid.NewGuid().ToString();
      await _authRepository.SaveRefreshTokenAsync(user.Id, refreshToken, deviceId);
    }

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

  public async Task<UserTokensDTO> Refresh(string? refreshToken, string? deviceId, UserDTO user)
  {
    if(string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(deviceId))
      throw new UnauthorizedAccessException("refreshToken or deviceId not found");
    bool alreadyExists = await _authRepository.CheckTokenAsync(refreshToken, deviceId);

    if (!alreadyExists)
      throw new UnauthorizedAccessException("Users with this refresh token and device id not found");

    var newRefreshToken = Guid.NewGuid().ToString();
    var newDeviceId = Guid.NewGuid().ToString();
    var newAccessToken = GenerateToken(user.Id.ToString(), user.Email, user.Role);

    await _authRepository.UpdateRefreshTokenAsync(refreshToken, newRefreshToken, newDeviceId);
    return new UserTokensDTO {
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
    if(string.IsNullOrEmpty(refreshToken))
      throw new UnauthorizedAccessException("Not registered");

    await _authRepository.DeleteRefreshTokenAsync(refreshToken);
  }

  private string GenerateToken(string userId, string email, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
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

  private static bool VerifyPassword(string password, string hashedPassword)
  {
    var passwordHasher = new PasswordHasher<UserDTO>();
    #pragma warning disable CS8625
    return passwordHasher.VerifyHashedPassword(null, hashedPassword, password) == PasswordVerificationResult.Success;
  }
}
