using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Core.Interfaces
{
  public interface IUserService
  {
    Task<UserDTO> GetUser(Guid id);

    Task<PaginatedResult<UserDTO>> GetUsers(PaginationParameters paginationParameters);

    Task<UserDTO> DeleteUser(Guid id);

    Task<UserDTO> UpdateUser(Guid id, UserDTO user);

    Task<UserDTO> CreateUser(UserCreateDTO manager);

  }
}