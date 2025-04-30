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
      var application = await _context.LoanApplications.Include(a => a.LoanApplicationStatusType)
          .FirstOrDefaultAsync(a => a.Id == id);

      if (application == null)
        throw new DatabaseOperationException(Operations.GetLoanApplication, new Exception("Loan application not found"));

      return new LoanApplicationDTO
      {
        Id = application.Id,
        UserId = application.UserId,
        Value = application.Value,
        Status = application.LoanApplicationStatusType.Name,
        Term = application.Term,
        CreatedAt = application.CreatedAt,
        ClientEmail = application.User.Email
      };
    }

    public async Task<PaginatedResult<LoanApplicationDTO>> GetLoanApplicationsByUserIdAsync(Guid userId, PaginationParameters parameters)
    {

      var query = _context.LoanApplications
      .Include(a => a.LoanApplicationStatusType)
          .Where(la => la.UserId == userId)
          .OrderByDescending(la => la.CreatedAt);

      var totalRecords = await query.CountAsync();

      var applications = await query
          .Skip(parameters.Offset * parameters.Size)
          .Include(app => app.RejectionReason)
          .Take(parameters.Size)
          .Select(application => new LoanApplicationDTO
          {
            Id = application.Id,
            UserId = application.UserId,
            Value = application.Value,
            Status = application.LoanApplicationStatusType.Name,
            Term = application.Term,
            CreatedAt = application.CreatedAt,
            RejectionReason = application.RejectionReason.reason,
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
          .FirstOrDefaultAsync(la => la.Id == id) ??
          throw new DatabaseOperationException(Operations.UpdateLoanApplication, new Exception("Loan application not found"));

      var approvedStatus = await _context.LoanApplicationStatusTypes.FirstOrDefaultAsync(a => a.Name == "Approved") ??
          throw new DatabaseOperationException(Operations.ApproveLoanApplication, new Exception("Not Found status"));

      application.LoanApplicationStatusTypeId = approvedStatus.Id;

      var getMoneyStatus = await _context.LoanStatusTypes.FirstOrDefaultAsync(a => a.Name == "GetMoney") ??
          throw new DatabaseOperationException(Operations.ApproveLoanApplication, new Exception("Not Found status"));

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
        LoanStatusTypeId = getMoneyStatus.Id,
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

      var approvedStatus = await _context.LoanApplicationStatusTypes.FirstOrDefaultAsync(a => a.Name == "Rejected") ??
    throw new DatabaseOperationException(Operations.ApproveLoanApplication, new Exception("Not Found status"));
      application.LoanApplicationStatusTypeId = approvedStatus.Id;

      RejectionReason rejectionReason = new RejectionReason { Id = Guid.NewGuid(), reason = reason };
      await _context.RejectionReasons.AddAsync(rejectionReason);
      application.RejectionReasonId = rejectionReason.Id;

      await _context.SaveChangesAsync();
    }

    public async Task<PaginatedResult<LoanApplicationDTO>> GetLoanApplicationsAsync(PaginationParameters parameters)
    {
      var query = _context.LoanApplications.Include(a => a.LoanApplicationStatusType).OrderByDescending(la => la.CreatedAt);

      var totalRecords = await query.CountAsync();

      var applications = await query
          .Skip(parameters.Offset * parameters.Size)
          .Take(parameters.Size)
          .Select(application => new LoanApplicationDTO
          {
            Id = application.Id,
            UserId = application.UserId,
            Value = application.Value,
            Status = application.LoanApplicationStatusType.Name,
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

      var moderationStatus = await _context.LoanApplicationStatusTypes.FirstOrDefaultAsync(a => a.Name == "Moderation") ??
      throw new DatabaseOperationException(Operations.ApproveLoanApplication, new Exception("Not Found status"));

      var application = new LoanApplication
      {
        Id = Guid.NewGuid(),
        UserId = applicationDto.UserId,
        Value = applicationDto.Value,
        LoanApplicationStatusTypeId = moderationStatus.Id,
        Term = applicationDto.Term,
        CreatedAt = DateTime.UtcNow
      };

      _context.LoanApplications.Add(application);
      await _context.SaveChangesAsync();

      application = await _context.LoanApplications
          .Include(la => la.User)
          .Include(la => la.LoanApplicationStatusType)
          .FirstOrDefaultAsync(la => la.Id == application.Id);

      return new LoanApplicationDTO
      {
        Id = application.Id,
        UserId = application.UserId,
        Value = application.Value,
        Status = application.LoanApplicationStatusType.Name,
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

      var applicationStatus = await _context.LoanApplicationStatusTypes.FirstOrDefaultAsync(a => a.Name == applicationDto.Status) ??
      throw new DatabaseOperationException(Operations.ApproveLoanApplication, new Exception("Not Found status"));

      application.Value = applicationDto.Value;
      application.LoanApplicationStatusTypeId = applicationStatus.Id;
      application.Term = applicationDto.Term;

      await _context.SaveChangesAsync();

      return new LoanApplicationDTO
      {
        Id = application.Id,
        UserId = application.UserId,
        Value = application.Value,
        Status = applicationStatus.Name,
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
