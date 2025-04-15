using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Database;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{

  public class LoanApplicationRepository : ILoanApplicationRepository
  {
    private readonly AppDbContext _context;

    public LoanApplicationRepository(AppDbContext context)
    {
      _context = context;
    }

    public async Task<LoanApplicationDTO> GetLoanApplicationAsync(Guid id)
    {
      var application = await _context.LoanApplications
          .FirstOrDefaultAsync(a => a.Id == id);

      if (application == null)
        throw new DatabaseOperationException(Operations.GetLoanApplication, new Exception("Loan application not found"));

      return new LoanApplicationDTO
      {
        Id = application.Id,
        UserId = application.UserId,
        Value = application.Value,
        Status = application.Status,
        Term = application.Term,
        CreatedAt = application.CreatedAt,
        ClientEmail = application.User.Email
      };
    }

    public async Task<PaginatedResult<LoanApplicationDTO>> GetLoanApplicationsByUserIdAsync(Guid userId, PaginationParameters parameters)
    {

      var query = _context.LoanApplications
          .Where(la => la.UserId == userId)
          .OrderByDescending(la => la.CreatedAt);

      var totalRecords = await query.CountAsync();

      var applications = await query
          .Skip(parameters.Offset * parameters.Size)
          .Take(parameters.Size)
          .Select(application => new LoanApplicationDTO
          {
            Id = application.Id,
            UserId = application.UserId,
            Value = application.Value,
            Status = application.Status,
            Term = application.Term,
            CreatedAt = application.CreatedAt,
            RejectionReason = application.RejectionReason,
            ClientEmail = application.User.Email
          })
          .ToListAsync();

      return new PaginatedResult<LoanApplicationDTO>
      {
        Data = applications,
        Total = totalRecords
      };
    }

    public async Task ApproveLoanApplicationAsync(Guid id)
    {
      var application = await _context.LoanApplications
          .FirstOrDefaultAsync(la => la.Id == id);

      if (application == null)
        throw new DatabaseOperationException(Operations.UpdateLoanApplication, new Exception("Loan application not found"));

      application.Status = LoanApplicationStatus.Approved;

      decimal percent = (decimal)(int)application.Term * 2;
      decimal multiplier = 1 + percent / 100m;

      var loan = await _context.Loans.AddAsync(new Loan
      {
        Id = Guid.NewGuid(),
        UserId = application.UserId,
        Percent = (int)percent,
        ValueToPay = application.Value,
        ValueToPayOnCurrentMonth = Math.Round(application.Value * multiplier / (int)application.Term, 2),
        CompletedValue = 0,
        NextPaymentDate = DateTime.UtcNow.AddMonths(1),
        LeftValue = Math.Round(application.Value * multiplier, 2),
        Status = LoanStatus.GetMoney,
        Term = application.Term,
        CreatedAt = DateTime.UtcNow
      });

      await _context.SaveChangesAsync();
    }

    public async Task RejectLoanApplicationAsync(Guid id, string reason)
    {
      var application = await _context.LoanApplications
          .FirstOrDefaultAsync(la => la.Id == id);

      if (application == null)
        throw new DatabaseOperationException(Operations.UpdateLoanApplication, new Exception("Loan application not found"));

      application.Status = LoanApplicationStatus.Rejected;
      application.RejectionReason = reason;

      await _context.SaveChangesAsync();
    }

    public async Task<PaginatedResult<LoanApplicationDTO>> GetLoanApplicationsAsync(PaginationParameters parameters)
    {
      var query = _context.LoanApplications.OrderByDescending(la => la.CreatedAt);

      var totalRecords = await query.CountAsync();

      var applications = await query
          .Skip(parameters.Offset * parameters.Size)
          .Take(parameters.Size)
          .Select(application => new LoanApplicationDTO
          {
            Id = application.Id,
            UserId = application.UserId,
            Value = application.Value,
            Status = application.Status,
            Term = application.Term,
            CreatedAt = application.CreatedAt,
            ClientEmail = application.User.Email
          })
          .ToListAsync();

      return new PaginatedResult<LoanApplicationDTO>
      {
        Data = applications,
        Total = totalRecords
      };
    }

    public async Task<LoanApplicationDTO> CreateLoanApplicationAsync(CreateLoanApplicationDTO applicationDto)
    {
      var application = new LoanApplication
      {
        Id = Guid.NewGuid(),
        UserId = applicationDto.UserId,
        Value = applicationDto.Value,
        Status = LoanApplicationStatus.Moderation,
        Term = applicationDto.Term,
        CreatedAt = DateTime.UtcNow
      };

      _context.LoanApplications.Add(application);
      await _context.SaveChangesAsync();

      application = await _context.LoanApplications
          .Include(la => la.User)
          .FirstOrDefaultAsync(la => la.Id == application.Id);

      return new LoanApplicationDTO
      {
        Id = application.Id,
        UserId = application.UserId,
        Value = application.Value,
        Status = application.Status,
        Term = application.Term,
        CreatedAt = application.CreatedAt,
        ClientEmail = application.User.Email
      };
    }

    public async Task<LoanApplicationDTO> UpdateLoanApplicationAsync(Guid id, LoanApplicationDTO applicationDto)
    {
      var application = await _context.LoanApplications.FirstOrDefaultAsync(la => la.Id == id);

      if (application == null)
        throw new DatabaseOperationException(Operations.UpdateLoanApplication, new Exception("Loan application not found"));

      application.Value = applicationDto.Value;
      application.Status = applicationDto.Status;
      application.Term = applicationDto.Term;

      await _context.SaveChangesAsync();

      return new LoanApplicationDTO
      {
        Id = application.Id,
        UserId = application.UserId,
        Value = application.Value,
        Status = application.Status,
        Term = application.Term,
        CreatedAt = application.CreatedAt,
        ClientEmail = application.User.Email
      };
    }

    public async Task DeleteLoanApplicationAsync(Guid id)
    {
      var application = await _context.LoanApplications.FindAsync(id);

      if (application == null)
        throw new DatabaseOperationException(Operations.DeleteLoanApplication, new Exception("Loan application not found"));

      _context.LoanApplications.Remove(application);
      await _context.SaveChangesAsync();
    }
  }
}
