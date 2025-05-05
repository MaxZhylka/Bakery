using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

public class SettingsRepository : ISettingsRepository
{
  private readonly AppDbContext _context;

  public SettingsRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<SettingsData> GetSettingsAsync()
  {
    var settings = await _context.SettingsData.FirstOrDefaultAsync();

    if (settings == null)
      throw new InvalidOperationException("Settings not found");

    return settings;
  }

  public async Task UpdateSettingsAsync(UpdateSettingsDto updated)
  {
    var settings = await _context.SettingsData.FirstOrDefaultAsync();

    if (settings == null)
      throw new InvalidOperationException("Settings not found");

    settings.StartReportDate = updated.StartReportDate;
    settings.EndReportDate = updated.EndReportDate;
    settings.BackupPath = updated.BackupPath;
    settings.HelperPhone = updated.HelperPhone;
    settings.HelperName = updated.HelperName;
    settings.CEOName = updated.CEOName;
    settings.CEOPhone = updated.CEOPhone;

    await _context.SaveChangesAsync();
  }
}
