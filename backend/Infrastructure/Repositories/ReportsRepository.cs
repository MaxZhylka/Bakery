using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Infrastructure.Database;
using backend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LoanApplicationReportDto>> GetApplicationsByMonthAsync(DateTime from, DateTime to)
    {
        return await _context.LoanApplications
            .Where(app => app.CreatedAt >= from && app.CreatedAt <= to)
            .GroupBy(app => new { app.CreatedAt.Year, app.CreatedAt.Month })
            .Select(g => new LoanApplicationReportDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Total = g.Count(),
                Rejected = g.Count(x => x.Status == LoanApplicationStatus.Rejected),
                RejectedPercent = (double)g.Count(x => x.Status == LoanApplicationStatus.Rejected) / g.Count() * 100
            })
            .ToListAsync();
    }

    public async Task<MoneyFlowDto> GetMoneyFlowAsync(DateTime from, DateTime to)
    {
        var issued = await _context.Loans
            .Where(l => l.CreatedAt >= from && l.CreatedAt <= to)
            .SumAsync(l => l.ValueToPay);

        var returned = await _context.Payments
            .Where(p => p.CreatedAt >= from && p.CreatedAt <= to)
            .SumAsync(p => p.Value);

        return new MoneyFlowDto
        {
            TotalIssued = issued,
            TotalReturned = returned
        };
    }

    public async Task<List<UserApplicationsDto>> GetApplicationsPerUserAsync(DateTime from, DateTime to)
    {
        return await _context.LoanApplications
            .Where(a => a.CreatedAt >= from && a.CreatedAt <= to)
            .GroupBy(a => new { a.User.Email })
            .Select(g => new UserApplicationsDto
            {
                Email = g.Key.Email,
                ApplicationsCount = g.Count()
            })
            .ToListAsync();
    }

    public async Task<List<MonthlyPaymentsDto>> GetMonthlyPaymentsAsync(DateTime from, DateTime to)
    {
        return await _context.Payments
            .Where(p => p.CreatedAt >= from && p.CreatedAt <= to)
            .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
            .Select(g => new MonthlyPaymentsDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                PaymentsCount = g.Count(),
                AverageAmount = g.Average(x => x.Value)
            })
            .ToListAsync();
    }

    public async Task<List<Loan>> GetCompletedLoansAsync()
    {
        return await _context.Loans
            .Where(l => l.Status == LoanStatus.Completed)
            .ToListAsync();
    }
}
