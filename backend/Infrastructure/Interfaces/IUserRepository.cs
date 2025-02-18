using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserAsync(Guid id);
        Task<PaginatedResult<UserDTO>> GetUsersAsync(PaginationParameters paginationParameters);
        Task<UserDTO> DeleteUserAsync(Guid id);
        Task<UserDTO> UpdateUserAsync(Guid id, UserDTO user);
        Task<UserDTO> CreateUserAsync(UserEntity user);
    }
}