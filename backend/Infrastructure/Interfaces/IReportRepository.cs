using backend.Core.DTOs;
using backend.Core.Entities;

namespace backend.Infrastructure.Interfaces
{
    public interface IReportRepository
    {
        Task<List<LoanApplicationReportDto>> GetApplicationsByMonthAsync(DateTime from, DateTime to);
        Task<MoneyFlowDto> GetMoneyFlowAsync(DateTime from, DateTime to);
        Task<List<UserApplicationsDto>> GetApplicationsPerUserAsync(DateTime from, DateTime to);
        Task<List<MonthlyPaymentsDto>> GetMonthlyPaymentsAsync(DateTime from, DateTime to);
        Task<List<Loan>> GetCompletedLoansAsync();
    }
}