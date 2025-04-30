using backend.Core.DTOs;
using backend.Core.Entities;

public interface ISettingsRepository
{
  Task<SettingsData> GetSettingsAsync();

  Task UpdateSettingsAsync(UpdateSettingsDto updated);
}
