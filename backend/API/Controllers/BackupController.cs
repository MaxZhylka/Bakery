using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Api.Controllers
{
  [ApiController]
  [Route("api/backup")]
  public class BackupController : ControllerBase
  {
    private readonly IBackupRepository _backupRepository;

    public BackupController(IBackupRepository backupRepository)
    {
      _backupRepository = backupRepository;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("download")]
    public async Task<IActionResult> DownloadBackup([FromQuery] string backupFolderPath)
    {
      if (string.IsNullOrEmpty(backupFolderPath))
      {
        return BadRequest("Шлях до папки резервного копіювання не може бути порожнім.");
      }

      byte[] backupBytes = await _backupRepository.CreateDatabaseBackupAsync(backupFolderPath);
      string fileName = $"backup_{System.DateTime.Now:yyyyMMddHHmmss}.bak";

      return File(backupBytes, "application/octet-stream", fileName);
    }

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
  }
}
