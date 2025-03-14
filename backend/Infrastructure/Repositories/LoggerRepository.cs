using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Core.Interfaces;
using backend.Core.Models;
using backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

namespace backend.Infrastructure.Repositories
{
    public class LoggerRepository : ILoggerRepository
    {
        private readonly AppDbContext _context;

        public LoggerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserActionLog> SaveLogAsync(UserActionDTO logDTO)
        {
            var log = new UserActionLog
            {
                Id = Guid.NewGuid(),
                UserId = logDTO.UserId,
                Operation = logDTO.Operation,
                Details = logDTO.Details,
                Timestamp = DateTime.UtcNow
            };

            _context.UserActionLogs.Add(log);
            await _context.SaveChangesAsync();

            return log;
        }

        public async Task<UserActionDTO[]> GetLogsByUserIdAsync(Guid userId)
        {
            var logs = await _context.UserActionLogs
                .Include(l => l.User)
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .Select(log => new UserActionDTO
                {
                    Id = log.Id,
                    UserId = log.UserId,
                    UserName = log.User.Name,
                    Email = log.User.Email,
                    UserRole = log.User.Role,
                    Operation = log.Operation,
                    Details = log.Details,
                    Timestamp = log.Timestamp
                })
                .ToArrayAsync();

            return logs;
        }

        public async Task<PaginatedResult<UserActionDTO>> GetAllLogsAsync(PaginationParameters parameters)
        {
            var query = _context.UserActionLogs
                .Include(l => l.User)
                .OrderByDescending(l => l.Timestamp);

            var totalRecords = await query.CountAsync();

            var logs = await query
                .Skip(parameters.Offset * parameters.Size)
                .Take(parameters.Size)
                .Select(log => new UserActionDTO
                {
                    Id = log.Id,
                    UserId = log.UserId,
                    UserName = log.User.Name,
                    Email = log.User.Email,
                    UserRole = log.User.Role,
                    Operation = log.Operation,
                    Details = log.Details,
                    Timestamp = log.Timestamp
                })
                .ToListAsync();

            return new PaginatedResult<UserActionDTO>
            {
                Data = logs,
                Total = totalRecords,
            };
        }

        public async Task<UserActionDTO> GetLogByIdAsync(Guid logId)
        {
            var log = await _context.UserActionLogs
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.Id == logId);

            if (log == null)
                throw new DatabaseOperationException(Operations.GetLog, new Exception("Log not found"));

            return new UserActionDTO
            {
                Id = log.Id,
                UserId = log.UserId,
                UserName = log.User.Name,
                Email = log.User.Email,
                UserRole = log.User.Role,
                Operation = log.Operation,
                Details = log.Details,
                Timestamp = log.Timestamp
            };
        }
    }
}
