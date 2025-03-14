using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;
using backend.Core.DTOs;
using Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using backend.Core.Models;

namespace backend.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LoggerController(ILoggerService loggerService) : ControllerBase
  {
    private readonly ILoggerService _loggerService = loggerService;

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<PaginatedResult<UserActionDTO>> GetAllLogs([FromQuery] PaginationParameters parameters)
    {
      return await _loggerService.GetAllLogs(parameters);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet("{id}")]
    public async Task<UserActionDTO> GetLogById(Guid id)
    {
      return await _loggerService.GetLogById(id);
    }

    
    [ErrorHandler]
    [Authorize(Roles = "Admin,Manager,User")]
    [HttpGet("/ByUserId/{userId}")]
    public async Task<UserActionDTO[]> GetLogByUserId(Guid userId)
    {
      return await _loggerService.GetLogsByUserId(userId);
    }
  }
}