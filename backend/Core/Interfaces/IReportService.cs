namespace backend.Core.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateLoanApplicationsByMonthPdfAsync(DateTime from, DateTime to);
        Task<byte[]> GenerateMoneyFlowReportPdfAsync(DateTime from, DateTime to);
        Task<byte[]> GenerateUserApplicationsReportPdfAsync(DateTime from, DateTime to);
        Task<byte[]> GenerateMonthlyPaymentsReportPdfAsync(DateTime from, DateTime to);
        Task<byte[]> GenerateCompletedLoansReportPdfAsync();
    }
}