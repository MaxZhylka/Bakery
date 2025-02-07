
using Microsoft.AspNetCore.Mvc;
using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using Microsoft.Net.Http.Headers;
using Core.Attributes;

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
      Secure = true,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    Response.Cookies.Append("DeviceId", tokens.DeviceId, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
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
  [HttpPost("Refresh")]
  public async Task<string> Refresh()
  {
    string? deviceId = HttpContext.Request.Cookies["DeviceId"];
    string? refreshToken = HttpContext.Request.Cookies["RefreshToken"];
    var tokens = await _authService.Refresh(refreshToken, deviceId);

    Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    Response.Cookies.Append("DeviceId", tokens.DeviceId, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
      Expires = DateTime.UtcNow.AddDays(30)
    });

    return tokens.AccessToken;
  }

  [ErrorHandler]
  [HttpPost("Logout")]
  public async Task<IActionResult> Logout()
  {
    string? refreshToken = HttpContext.Request.Cookies["RefreshToken"];
    await _authService.Logout(refreshToken);
    Response.Cookies.Delete("RefreshToken");
    Response.Cookies.Delete("DeviceId");
    return Ok();
  }

}