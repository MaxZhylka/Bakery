using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserAsync(Guid id);
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO> DeleteUserAsync(Guid id);
        Task<UserDTO> UpdateUserAsync(Guid id, UserDTO user);
        Task<UserDTO> CreateAdminAsync(UserEntity user);
        Task<UserDTO> CreateManagerAsync(UserEntity user);
    }
}