using backend.Core.DTOs;

namespace backend.Infrastructure.Interfaces
{
  public interface IReportRepository
  {
    Task<IEnumerable<ProductReportDto>> GetReportByProductAsync();
    Task<IEnumerable<CustomerReportDto>> GetReportByCustomerAsync();
    Task<IEnumerable<OrderReportDto>> GetAllOrdersReportAsync();
    Task<IEnumerable<OrderTrendsByCustomerDto>> GetOrderTrendsByCustomerAsync();
    Task<IEnumerable<OrderTrendsByProductDto>> GetOrderTrendsByProductAsync();
  }
}