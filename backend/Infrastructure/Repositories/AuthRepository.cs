using backend.Core.Entities;
using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;
using backend.Infrastructure.Database;

namespace backend.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            return user;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);

            if (token?.User == null)
                throw new UserNotFoundByRefreshException();

            return token.User;
        }

        public async Task<RefreshTokens> SaveRefreshTokenAsync(Guid userId, string refreshToken, string deviceId)
        {
            var token = new RefreshTokens
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RefreshToken = refreshToken,
                DeviceId = deviceId,
                Expiration = DateTime.UtcNow.AddDays(30)
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<bool> CheckTokenByDeviceIdAsync(string deviceId)
        {
            return await _context.RefreshTokens
                .AnyAsync(rt => rt.DeviceId == deviceId && rt.Expiration > DateTime.UtcNow);
        }

        public async Task<UserDTO?> CheckTokenAsync(string refreshToken, string deviceId)
        {
            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.DeviceId == deviceId &&
                                           rt.RefreshToken == refreshToken &&
                                           rt.Expiration > DateTime.UtcNow);

            if (token?.User == null)
                return null;

            return new UserDTO
            {
                Id = token.User.Id,
                Name = token.User.Name,
                Email = token.User.Email,
                Role = token.User.Role
            };
        }

        public async Task<RefreshTokens> UpdateRefreshTokenAsync(string oldRefreshToken, string refreshToken, string deviceId)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.RefreshToken == oldRefreshToken);

            if (token == null)
                throw new KeyNotFoundException("Refresh token not found for update");

            token.RefreshToken = refreshToken;
            token.DeviceId = deviceId;
            token.Expiration = DateTime.UtcNow.AddDays(30);

            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<RefreshTokens> UpdateRefreshTokenByIdAsync(Guid userId, string refreshToken, string oldDeviceId, string deviceId)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.DeviceId == oldDeviceId);

            if (token == null)
                throw new KeyNotFoundException("Refresh token not found for update by id");

            token.RefreshToken = refreshToken;
            token.DeviceId = deviceId;
            token.Expiration = DateTime.UtcNow.AddDays(30);

            await _context.SaveChangesAsync();

            return token;
        }

        public async Task DeleteRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);

            if (token == null)
                throw new KeyNotFoundException("Refresh token not found for deletion");

            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }
}
