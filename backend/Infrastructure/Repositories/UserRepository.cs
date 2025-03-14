using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;
using backend.Core.Entities;
using backend.Infrastructure.Interfaces;

namespace backend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> GetUserAsync(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<PaginatedResult<UserDTO>> GetUsersAsync(PaginationParameters paginationParameters)
        {
            var query = _context.Users.OrderBy(u => u.Name);

            var total = await query.CountAsync();

            var users = await query
                .Skip(paginationParameters.Offset * paginationParameters.Size)
                .Take(paginationParameters.Size)
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                })
                .ToArrayAsync();

            return new PaginatedResult<UserDTO>
            {
                Data = users,
                Total = total
            };
        }

        public async Task<UserDTO> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDTO> UpdateUserAsync(Guid id, UserDTO updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Role = updatedUser.Role;

            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDTO> CreateUserAsync(UserWithCredentialsDTO newUser)
        {
            var user = new User
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email,
                Role = newUser.Role,
                Password = newUser.Password,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
