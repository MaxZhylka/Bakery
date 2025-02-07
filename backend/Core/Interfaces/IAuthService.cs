using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Core.Interfaces
{
    public interface IAuthService
    {
        Task<UserTokensDTO> Login(string email, string password, string? deviceId);
        Task<UserTokensDTO> Refresh(string? refreshToken, string? deviceId);
        Task Logout(string? refreshToken);

    }
}