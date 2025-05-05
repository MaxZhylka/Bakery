
namespace backend.Core.Entities
{
  public class SettingsData
  {
    public Guid Id { get; set; }
    public DateTime StartReportDate { get; set; }
    public DateTime EndReportDate { get; set; }
    public required string BackupPath { get; set; }

    public required string CEOName { get; set; }
    public required string CEOPhone { get; set; }
    public required string HelperName { get; set; }
    public required string HelperPhone { get; set; }
  }
}
