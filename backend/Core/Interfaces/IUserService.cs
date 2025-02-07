using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Core.Interfaces
{
  public interface IUserService
  {
    Task<UserDTO> GetUser(Guid id);

    Task<IEnumerable<UserDTO>> GetUsers();

    Task<UserDTO> DeleteUser(Guid id);

    Task<UserDTO> UpdateUser(Guid id, UserDTO user);

    Task<UserDTO> CreateUser(UserCreateDTO manager);

  }
}