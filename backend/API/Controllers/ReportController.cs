using backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reports")]
public class ReportController(IReportService reportService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    [HttpGet("by-product")]
    public async Task<IActionResult> GetReportByProduct()
    {
        var reportBytes = await _reportService.GenerateProductReportAsync();
        return File(reportBytes, "application/pdf", "ProductReport.pdf");
    }

    [HttpGet("by-customer")]
    public async Task<IActionResult> GetReportByCustomer()
    {
        var reportBytes = await _reportService.GenerateCustomerReportAsync();
        return File(reportBytes, "application/pdf", "CustomerReport.pdf");
    }

    [HttpGet("all-orders")]
    public async Task<IActionResult> GetAllOrdersReport()
    {
        var reportBytes = await _reportService.GenerateAllOrdersReportAsync();
        return File(reportBytes, "application/pdf", "AllOrdersReport.pdf");
    }

    [HttpGet("trends-by-customer")]
    public async Task<IActionResult> GetOrderTrendsByCustomer()
    {
        var reportBytes = await _reportService.GenerateOrderTrendsByCustomerReportAsync();
        return File(reportBytes, "application/pdf", "OrderTrendsByCustomer.pdf");
    }

    [HttpGet("trends-by-product")]
    public async Task<IActionResult> GetOrderTrendsByProduct()
    {
        var reportBytes = await _reportService.GenerateOrderTrendsByProductReportAsync();
        return File(reportBytes, "application/pdf", "OrderTrendsByProduct.pdf");
    }
}
