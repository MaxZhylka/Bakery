using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Models;

namespace backend.Core.Interfaces
{
    public interface ILoggerRepository
    {
        Task<UserActionLog> SaveLogAsync(UserActionDTO log);

        Task<UserActionDTO[]> GetLogsByUserIdAsync(Guid userId);

        Task<PaginatedResult<UserActionDTO>> GetAllLogsAsync(PaginationParameters parameters);

        Task<UserActionDTO> GetLogByIdAsync(Guid logId);
    }
}
