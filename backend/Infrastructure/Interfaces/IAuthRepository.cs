using backend.Core.DTOs;
using backend.Core.Entities;

namespace backend.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<RefreshTokens> SaveRefreshTokenAsync(Guid userId, string refreshToken, string deviceId);

        Task<UserDTO?> CheckTokenAsync(string refreshToken, string deviceId);
        Task<bool> CheckTokenByDeviceIdAsync(string deviceId);
        Task<RefreshTokens> UpdateRefreshTokenAsync(string oldRefreshToken, string refreshToken, string deviceId);
        Task<RefreshTokens> UpdateRefreshTokenByIdAsync(Guid userId, string refreshToken, string oldDeviceId, string deviceId);
        Task DeleteRefreshTokenAsync(string refreshToken);
    }
}