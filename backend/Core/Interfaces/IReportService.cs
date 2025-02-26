namespace backend.Core.Interfaces
{
  public interface IReportService
  {
    Task<byte[]> GenerateProductReportAsync();
    Task<byte[]> GenerateCustomerReportAsync();
    Task<byte[]> GenerateAllOrdersReportAsync();
    Task<byte[]> GenerateOrderTrendsByCustomerReportAsync();
    Task<byte[]> GenerateOrderTrendsByProductReportAsync();
  }
}