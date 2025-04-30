using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Database;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

public class LoanRepository : ILoanRepository
{
  private readonly AppDbContext _context;

  public LoanRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<LoanDTO> GetLoanAsync(Guid id)
  {
    var loan = await _context.Loans
        .Include(l => l.User)
        .Include(l => l.LoanStatusType)
        .FirstOrDefaultAsync(l => l.Id == id);

    if (loan == null)
      throw new DatabaseOperationException(Operations.GetLoan, new Exception("Loan not found"));

    return new LoanDTO
    {
      Id = loan.Id,
      UserId = loan.UserId,
      Percent = loan.Percent,
      ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
      ValueToPay = loan.ValueToPay,
      Status = loan.LoanStatusType.Name,
      CreatedAt = loan.CreatedAt,
      ClientEmail = loan.User.Email,
      CompletedValue = loan.CompletedValue,
      NextPaymentDate = loan.NextPaymentDate,
      LeftValue = loan.LeftValue,
      Term = loan.Term
    };
  }

  public async Task<PaginatedResult<LoanDTO>> GetLoansByUserIdAsync(Guid userId, PaginationParameters parameters)
  {
    var query = _context.Loans
        .Where(l => l.UserId == userId)
        .Include(l => l.User)
        .Include(l => l.LoanStatusType)
        .OrderByDescending(l => l.CreatedAt);

    var totalRecords = await query.CountAsync();

    var loans = await query
        .Skip(parameters.Offset * parameters.Size)
        .Take(parameters.Size)
        .Select(loan => new LoanDTO
        {
          Id = loan.Id,
          UserId = loan.UserId,
          Percent = loan.Percent,
          ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
          ValueToPay = loan.ValueToPay,
          Status = loan.LoanStatusType.Name,
          CreatedAt = loan.CreatedAt,
          ClientEmail = loan.User.Email,
          CompletedValue = loan.CompletedValue,
          NextPaymentDate = loan.NextPaymentDate,
          LeftValue = loan.LeftValue,
          Term = loan.Term
        })
        .ToListAsync();

    return new PaginatedResult<LoanDTO>
    {
      Data = loans,
      Total = totalRecords
    };
  }

  public async Task<PaginatedResult<LoanDTO>> GetLoansAsync(PaginationParameters parameters)
  {
    var query = _context.Loans
        .Include(l => l.User)
        .Include(l => l.LoanStatusType)
        .OrderByDescending(l => l.CreatedAt);

    var totalRecords = await query.CountAsync();

    var loans = await query
        .Skip(parameters.Offset * parameters.Size)
        .Take(parameters.Size)
        .Select(loan => new LoanDTO
        {
          Id = loan.Id,
          UserId = loan.UserId,
          Percent = loan.Percent,
          ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
          ValueToPay = loan.ValueToPay,
          Status = loan.LoanStatusType.Name,
          CreatedAt = loan.CreatedAt,
          ClientEmail = loan.User.Email,
          CompletedValue = loan.CompletedValue,
          NextPaymentDate = loan.NextPaymentDate,
          LeftValue = loan.LeftValue,
          Term = loan.Term
        })
        .ToListAsync();

    return new PaginatedResult<LoanDTO>
    {
      Data = loans,
      Total = totalRecords
    };
  }

  public async Task<LoanDTO> CreateLoanAsync(LoanDTO loanDto)
  {
    var status = await _context.LoanStatusTypes.FirstOrDefaultAsync(s => s.Name == loanDto.Status) ??
        throw new DatabaseOperationException(Operations.CreateLoan, new Exception("Loan status not found"));

    var loan = new Loan
    {
      Id = Guid.NewGuid(),
      UserId = loanDto.UserId,
      Percent = loanDto.Percent,
      ValueToPayOnCurrentMonth = loanDto.ValueToPayOnCurrentMonth,
      ValueToPay = loanDto.ValueToPay,
      CompletedValue = loanDto.CompletedValue,
      NextPaymentDate = loanDto.NextPaymentDate,
      LeftValue = loanDto.LeftValue,
      LoanStatusTypeId = status.Id,
      CreatedAt = DateTime.UtcNow,
      Term = loanDto.Term
    };

    _context.Loans.Add(loan);
    await _context.SaveChangesAsync();

    var created = await _context.Loans
        .Include(l => l.User)
        .Include(l => l.LoanStatusType)
        .FirstOrDefaultAsync(l => l.Id == loan.Id);

    return new LoanDTO
    {
      Id = created.Id,
      UserId = created.UserId,
      Percent = created.Percent,
      ValueToPayOnCurrentMonth = created.ValueToPayOnCurrentMonth,
      ValueToPay = created.ValueToPay,
      Status = created.LoanStatusType.Name,
      CreatedAt = created.CreatedAt,
      ClientEmail = created.User.Email,
      CompletedValue = created.CompletedValue,
      NextPaymentDate = created.NextPaymentDate,
      LeftValue = created.LeftValue,
      Term = created.Term
    };
  }

  public async Task<LoanDTO> UpdateLoanAsync(Guid id, LoanDTO updatedLoan)
  {
    var loan = await _context.Loans.FirstOrDefaultAsync(l => l.Id == id);

    if (loan == null)
      throw new DatabaseOperationException(Operations.UpdateLoan, new Exception("Loan not found"));

    var status = await _context.LoanStatusTypes.FirstOrDefaultAsync(s => s.Name == updatedLoan.Status) ??
    throw new DatabaseOperationException(Operations.CreateLoan, new Exception("Loan status not found"));


    loan.Percent = updatedLoan.Percent;
    loan.ValueToPayOnCurrentMonth = updatedLoan.ValueToPayOnCurrentMonth;
    loan.ValueToPay = updatedLoan.ValueToPay;
    loan.LoanStatusTypeId = status.Id;

    await _context.SaveChangesAsync();

    return new LoanDTO
    {
      Id = loan.Id,
      UserId = loan.UserId,
      Percent = loan.Percent,
      ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
      ValueToPay = loan.ValueToPay,
      Status = status.Name,
      CreatedAt = loan.CreatedAt,
      ClientEmail = loan.User.Email,
      CompletedValue = loan.CompletedValue,
      NextPaymentDate = loan.NextPaymentDate,
      LeftValue = loan.LeftValue,
    };
  }

  public async Task<LoanDTO> GetMoneyByLoanIdAsync(Guid id)
  {
    var loan = await _context.Loans.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == id);

    if (loan == null)
      throw new DatabaseOperationException(Operations.GetMoneyByLoanId, new Exception("Loan not found"));

    var status = await _context.LoanStatusTypes.FirstOrDefaultAsync(s => s.Name == "Active") ??
    throw new DatabaseOperationException(Operations.CreateLoan, new Exception("Loan status not found"));

    loan.LoanStatusTypeId = status.Id;

    await _context.SaveChangesAsync();

    return new LoanDTO
    {
      Id = loan.Id,
      UserId = loan.UserId,
      Percent = loan.Percent,
      ValueToPayOnCurrentMonth = loan.ValueToPayOnCurrentMonth,
      ValueToPay = loan.ValueToPay,
      Status = status.Name,
      CreatedAt = loan.CreatedAt,
      ClientEmail = loan.User.Email,
      CompletedValue = loan.CompletedValue,
      NextPaymentDate = loan.NextPaymentDate,
      LeftValue = loan.LeftValue,
    };
  }

  public async Task DeleteLoanAsync(Guid id)
  {
    var loan = await _context.Loans.FindAsync(id);

    if (loan == null)
      throw new DatabaseOperationException(Operations.DeleteLoan, new Exception("Loan not found"));

    _context.Loans.Remove(loan);
    await _context.SaveChangesAsync();
  }
}
