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

        private readonly PasswordHasher<UserEntity> _passwordHasher = new();
        public async Task<UserDTO> GetUser(Guid id)
        {
            var user = await _userRepository.GetUserAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
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

        public async Task<UserDTO> CreateAdmin(UserCreateDTO user)
        {
            #pragma warning disable CS8625
            var hashedPassword = _passwordHasher.HashPassword(null, user.Password);
            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                Password = hashedPassword,
                Role = "Admin",
                CreatedAt = DateTime.Now
            };
            var createdUser = await _userRepository.CreateAdminAsync(userEntity);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<UserDTO> CreateManager(UserCreateDTO user)
        {
            #pragma warning disable CS8625
            var hashedPassword = _passwordHasher.HashPassword(null, user.Password);


            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                Email = user.Email,
                Password = hashedPassword,
                Role = "Manager",
                CreatedAt = DateTime.Now
            };
            var createdUser = await _userRepository.CreateManagerAsync(userEntity);
            return _mapper.Map<UserDTO>(createdUser);
        }
    }
}