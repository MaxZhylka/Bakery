using backend.Core.DTOs;
using backend.Core.Interfaces;
using backend.Core.Models;
using backend.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace backend.Core.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        private readonly PasswordHasher<string> _passwordHasher = new();
        public async Task<UserDTO> GetUser(Guid id)
        {
            var user = await _userRepository.GetUserAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<PaginatedResult<UserDTO>> GetUsers(PaginationParameters paginationParameters)
        {
            return await _userRepository.GetUsersAsync(paginationParameters);

        }

        public async Task<UserDTO> DeleteUser(Guid id)
        {
            var deletedUser = await _userRepository.DeleteUserAsync(id);
            return _mapper.Map<UserDTO>(deletedUser);
        }

        public async Task<UserDTO> UpdateUser(Guid id, UserDTO user)
        {
            return await _userRepository.UpdateUserAsync(id, user);
        }

        public async Task<UserDTO> CreateUser(UserCreateDTO user)
        {

            var hashedPassword = _passwordHasher.HashPassword(user.Email, user.Password);
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                Password = hashedPassword,
                Role = user.Role,
                CreatedAt = DateTime.Now
            };
            var createdUser = await _userRepository.CreateUserAsync(userEntity);
            return _mapper.Map<UserDTO>(createdUser);
        }
    }
}