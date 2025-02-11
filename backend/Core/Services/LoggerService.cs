using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Core.Interfaces;
using backend.Core.Models;

namespace backend.Core.Services
{
  public class LoggerService(ILoggerRepository loggerRepository) : ILoggerService
  {
    private readonly ILoggerRepository _loggerRepository = loggerRepository;

    public async Task<UserActionDTO[]> GetAllLogs(PaginationParameters parameters)
    {
      return await _loggerRepository.GetAllLogsAsync(parameters);
    }

    public async Task<UserActionDTO> GetLogById(Guid LogId)
    {
      return await _loggerRepository.GetLogByIdAsync(LogId);
    }

    public async Task<UserActionDTO[]> GetLogsByUserID(Guid userId)
    {
      return await _loggerRepository.GetLogsByUserIdAsync(userId);
    }

    public Task<UserActionLog> LogAction(Guid userId, Operations operation, string details)
    {
      UserActionLog userActionLog = new() { UserId = userId, Operation = operation, Details = details };
      return Task.Run(() => _loggerRepository.SaveLogAsync(userActionLog));
    }
  }
}