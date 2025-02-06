using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
  private readonly IUserService _userService = userService;


  [Authorize(Roles = "Admin")]
  [HttpPost("admin")]
  public async Task<UserDTO> CreateAdmin([FromBody] UserCreateDTO admin)
  {
    return await _userService.CreateAdmin(admin);
  }

  [HttpPost("manager")]
  public async Task<UserDTO> CreateManager([FromBody] UserCreateDTO  user)
  {
    return await _userService.CreateManager(user);
  }

  [HttpPut("user/{id}")]
  public async Task<UserDTO> UpdateUser(Guid id, [FromBody] UserDTO user)
  {
    return await _userService.UpdateUser(id, user);
  }

  [HttpGet("users")]
  public async Task<IEnumerable<UserDTO>> GetUsers()
  {
    return await _userService.GetUsers();
  }

    [HttpGet("users/{id}")]
  public async Task<UserDTO> GetUsers(Guid id)
  {
    return await _userService.GetUser(id);
  }

  [HttpDelete("user/{id}")]
  public async Task<UserDTO> DeleteUser(Guid id)
  {
    return await _userService.DeleteUser(id);
  }
}