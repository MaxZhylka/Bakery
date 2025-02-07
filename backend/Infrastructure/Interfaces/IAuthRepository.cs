using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<UserEntity> GetUserByRefreshTokenAsync(string refreshToken);
        Task<RefreshTokenEntity> SaveRefreshTokenAsync(Guid userId, string refreshToken, string deviceId);

        Task<UserDTO?> CheckTokenAsync(string refreshToken, string deviceId);
        Task<bool> CheckTokenByDeviceIdAsync(string deviceId);
        Task<RefreshTokenEntity> UpdateRefreshTokenAsync(string oldRefreshToken, string refreshToken, string deviceId);
        Task<RefreshTokenEntity> UpdateRefreshTokenByIdAsync(Guid userId, string refreshToken, string oldDeviceId, string deviceId);
        Task DeleteRefreshTokenAsync(string refreshToken);
    }
}