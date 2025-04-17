using Microsoft.AspNetCore.Mvc;
using backend.Core.Interfaces;

namespace backend.Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("loan-applications-month")]
        public async Task<IActionResult> GetLoanApplicationsByMonth([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var reportBytes = await _reportService.GenerateLoanApplicationsByMonthPdfAsync(from, to);
            return File(reportBytes, "application/pdf", $"LoanApplications_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf");
        }

        [HttpGet("money-flow")]
        public async Task<IActionResult> GetMoneyFlowReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var reportBytes = await _reportService.GenerateMoneyFlowReportPdfAsync(from, to);
            return File(reportBytes, "application/pdf", $"MoneyFlow_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf");
        }

        [HttpGet("applications-per-user")]
        public async Task<IActionResult> GetUserApplicationsReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var reportBytes = await _reportService.GenerateUserApplicationsReportPdfAsync(from, to);
            return File(reportBytes, "application/pdf", $"ApplicationsPerUser_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf");
        }

        [HttpGet("monthly-payments")]
        public async Task<IActionResult> GetMonthlyPaymentsReport([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var reportBytes = await _reportService.GenerateMonthlyPaymentsReportPdfAsync(from, to);
            return File(reportBytes, "application/pdf", $"MonthlyPayments_{from:yyyyMMdd}_{to:yyyyMMdd}.pdf");
        }

        [HttpGet("completed-loans")]
        public async Task<IActionResult> GetCompletedLoansReport()
        {
            var reportBytes = await _reportService.GenerateCompletedLoansReportPdfAsync();
            return File(reportBytes, "application/pdf", "CompletedLoansReport.pdf");
        }
    }
}
