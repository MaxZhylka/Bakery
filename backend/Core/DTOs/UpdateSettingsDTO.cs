
namespace backend.Core.DTOs
{
  public class UpdateSettingsDto
  {
    public DateTime StartReportDate { get; set; }
    public DateTime EndReportDate { get; set; }
    public required string BackupPath { get; set; }
  }
}
