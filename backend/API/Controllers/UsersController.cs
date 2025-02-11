using backend.Core.DTOs;
using backend.Core.Interfaces;
using Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
  private readonly IUserService _userService = userService;


  [ErrorHandler]
  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<UserDTO> CreateUser([FromBody] UserCreateDTO admin)
  {
    return await _userService.CreateUser(admin);
  }

  [ErrorHandler]
  [Authorize(Roles = "Admin")]
  [HttpPut("{id}")]
  public async Task<UserDTO> UpdateUser(Guid id, [FromBody] UserDTO user)
  {
    return await _userService.UpdateUser(id, user);
  }

  [ErrorHandler]
  [Authorize(Roles = "Admin")]
  [HttpGet]
  public async Task<IEnumerable<UserDTO>> GetUsers()
  {
    return await _userService.GetUsers();
  }

  [ErrorHandler]
  [Authorize(Roles = "Admin")]
  [HttpGet("{id}")]
  public async Task<UserDTO> GetUsers(Guid id)
  {
    return await _userService.GetUser(id);
  }

  [ErrorHandler]
  [Authorize(Roles = "Admin")]
  [HttpDelete("{id}")]
  public async Task<UserDTO> DeleteUser(Guid id)
  {
    return await _userService.DeleteUser(id);
  }
}