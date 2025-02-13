
using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Core.Models;


namespace backend.Core.Interfaces {
  public interface ILoggerService 
  {
    Task LogAction(Guid userId, Operations operation, string details);

    Task<UserActionDTO[]> GetLogsByUserId(Guid userId);

    Task<UserActionDTO[]> GetAllLogs(PaginationParameters parameters);

    Task<UserActionDTO> GetLogById(Guid logId);

  }
}