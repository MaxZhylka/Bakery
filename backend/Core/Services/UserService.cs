using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace backend.Core.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        private readonly PasswordHasher<string> _passwordHasher = new();
        public async Task<UserDTO> GetUser(Guid id)
        {
            return  await _userRepository.GetUserAsync(id);
        }

        public async Task<PaginatedResult<UserDTO>> GetUsers(PaginationParameters paginationParameters)
        {
            return await _userRepository.GetUsersAsync(paginationParameters);

        }

        public async Task<UserDTO> DeleteUser(Guid id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<UserDTO> UpdateUser(Guid id, UserDTO user)
        {
            return await _userRepository.UpdateUserAsync(id, user);
        }

        public async Task<UserDTO> CreateUser(UserCreateDTO user)
        {

            var hashedPassword = _passwordHasher.HashPassword(user.Email, user.Password);
            var userEntity = new UserWithCredentialsDTO
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                Password = hashedPassword,
                Role = user.Role,
                CreatedAt = DateTime.Now
            };
            return await _userRepository.CreateUserAsync(userEntity);
        }
    }
}