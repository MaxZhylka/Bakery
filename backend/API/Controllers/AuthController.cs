
using Microsoft.AspNetCore.Mvc;
using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using Microsoft.Net.Http.Headers;
using Core.Attributes;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
  private readonly IAuthService _authService = authService;

  [ErrorHandler]
  [HttpPost("Login")]
  public async Task<UserDTO> Login([FromBody] CredentialsEntity credentials)
  {
    string? deviceId = HttpContext.Request.Cookies["DeviceId"];
    UserTokensDTO tokens = await _authService.Login(credentials.Email, credentials.Password, deviceId);

    Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = false,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    Response.Cookies.Append("DeviceId", tokens.DeviceId, new CookieOptions
    {
      HttpOnly = true,
      Secure = false,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    return new UserDTO
    {
      Id = tokens.Id,
      Email = tokens.Email,
      Name = tokens.Name,
      Role = tokens.Role,
      AccessToken = tokens.AccessToken
    };
  }

  [ErrorHandler]
  [HttpPost("Register")]
  public async Task<UserDTO> Register([FromBody] RegisterCredentialsEntity credentials)
  {
    UserTokensDTO tokens = await _authService.Register(credentials);

    Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = false,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    Response.Cookies.Append("DeviceId", tokens.DeviceId, new CookieOptions
    {
      HttpOnly = true,
      Secure = false,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    return new UserDTO
    {
      Id = tokens.Id,
      Email = tokens.Email,
      Name = tokens.Name,
      Role = tokens.Role,
      AccessToken = tokens.AccessToken
    };
  }

  [ErrorHandler]
  [HttpGet("Refresh")]
  public async Task<UserDTO> Refresh()
  {
    string? deviceId = HttpContext.Request.Cookies["DeviceId"];
    string? refreshToken = HttpContext.Request.Cookies["RefreshToken"];

    UserTokensDTO userWithTokens = await _authService.Refresh(refreshToken, deviceId);

    Response.Cookies.Append("RefreshToken", userWithTokens.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = false,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    Response.Cookies.Append("DeviceId", userWithTokens.DeviceId, new CookieOptions
    {
      HttpOnly = true,
      Secure = false,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    return new UserDTO
    {
      Id = userWithTokens.Id,
      Email = userWithTokens.Email,
      Name = userWithTokens.Name,
      Role = userWithTokens.Role,
      AccessToken = userWithTokens.AccessToken
    };
  }

  [Authorize]
  [ErrorHandler]
  [HttpGet("Logout")]
  public async Task<IActionResult> Logout()
  {
    string? refreshToken = HttpContext.Request.Cookies["RefreshToken"];
    await _authService.Logout(refreshToken);
    Response.Cookies.Delete("RefreshToken");
    Response.Cookies.Delete("DeviceId");
    return Ok();
  }

}