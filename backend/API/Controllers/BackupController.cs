using System.IO;
using System.Threading.Tasks;
using backend.Core.DTOs;
using backend.Core.Entities;
using Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers
{
  [ApiController]
  [Authorize(Roles = "Admin")]
  [Route("api/backup")]
  public class BackupController : ControllerBase
  {
    private readonly IBackupRepository _backupRepository;
    private readonly ISettingsRepository _settingsRepository;

    public BackupController(IBackupRepository backupRepository, ISettingsRepository settingsRepository)
    {
      _backupRepository = backupRepository;
      _settingsRepository = settingsRepository;
    }


    [ErrorHandler]
    [Authorize(Roles = "Admin")]
    [HttpGet("download")]
    public async Task<IActionResult> DownloadBackup()
    {
      
      var settings = await _settingsRepository.GetSettingsAsync();

      byte[] backupBytes = await _backupRepository.CreateDatabaseBackupAsync(settings.BackupPath);
      string fileName = $"backup_{System.DateTime.Now:yyyyMMddHHmmss}.bak";

      return File(backupBytes, "application/octet-stream", fileName);
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin")]
    [HttpPost("restore")]
    public async Task<IActionResult> RestoreBackup([FromForm] IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return BadRequest("Файл не дійсний");
      }

      byte[] fileContent;
      using (var memoryStream = new MemoryStream())
      {
        await file.CopyToAsync(memoryStream);
        fileContent = memoryStream.ToArray();
      }

      await _backupRepository.RestoreDatabaseBackupAsync(fileContent);
      return Ok(new { message = "База даних успішно відновлена." });
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin")]
    [HttpGet("settings")]
    public async Task<SettingsData> GetSettings()
    {
      return await _settingsRepository.GetSettingsAsync();
    }

    [ErrorHandler]
    [Authorize(Roles = "Admin")]
    [HttpPut("settings")]
    public async Task UpdateSettings([FromBody] UpdateSettingsDto settings)
    {
      await _settingsRepository.UpdateSettingsAsync(settings);
    }

  }
}
