// backend/Infrastructure/Repositories/PaymentRepository.cs
using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Database;
using backend.Infrastructure.Repositories;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
  private readonly AppDbContext _context;

  public PaymentRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<PaymentDTO> GetPaymentAsync(Guid id)
  {
    var payment = await _context.Payments
      .Include(p => p.User)
      .Include(p => p.PaymentStatusType)
      .FirstOrDefaultAsync(p => p.Id == id);

    if (payment == null)
      throw new DatabaseOperationException(Operations.GetPayment, new Exception("Payment not found"));

    return new PaymentDTO
    {
      Id = payment.Id,
      UserId = payment.UserId,
      LoanId = payment.LoanId,
      Value = payment.Value,
      Status = payment.PaymentStatusType.Name,
      CreatedAt = payment.CreatedAt
    };
  }

  public async Task<PaginatedResult<PaymentDTO>> GetPaymentsAsync(PaginationParameters parameters)
  {
    var query = _context.Payments
      .Include(p => p.PaymentStatusType)
      .OrderByDescending(p => p.CreatedAt);

    var total = await query.CountAsync();

    var items = await query
      .Skip(parameters.Offset * parameters.Size)
      .Take(parameters.Size)
      .Select(p => new PaymentDTO
      {
        Id = p.Id,
        UserId = p.UserId,
        LoanId = p.LoanId,
        Value = p.Value,
        Status = p.PaymentStatusType.Name,
        CreatedAt = p.CreatedAt
      })
      .ToListAsync();

    return new PaginatedResult<PaymentDTO>
    {
      Data = items,
      Total = total
    };
  }

  public async Task<PaginatedResult<PaymentDTO>> GetPaymentsByUserIdAsync(Guid userId, PaginationParameters parameters)
  {
    var query = _context.Payments
      .Where(p => p.UserId == userId)
      .Include(p => p.PaymentStatusType)
      .OrderByDescending(p => p.CreatedAt);

    var total = await query.CountAsync();

    var items = await query
      .Skip(parameters.Offset * parameters.Size)
      .Take(parameters.Size)
      .Select(p => new PaymentDTO
      {
        Id = p.Id,
        UserId = p.UserId,
        LoanId = p.LoanId,
        Value = p.Value,
        Status = p.PaymentStatusType.Name,
        CreatedAt = p.CreatedAt
      })
      .ToListAsync();

    return new PaginatedResult<PaymentDTO>
    {
      Data = items,
      Total = total
    };
  }

  public async Task<PaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto)
  {

    var statusType = await _context.PaymentStatusTypes
  .FirstOrDefaultAsync(s => s.Name == "Completed")
  ?? throw new DatabaseOperationException(Operations.UpdatePayment, new Exception("Payment status not found"));

    var loanStatusType = await _context.LoanStatusTypes
  .FirstOrDefaultAsync(s => s.Name == "Completed")
  ?? throw new DatabaseOperationException(Operations.UpdatePayment, new Exception("Payment status not found"));

    var payment = new Payment
    {
      Id = Guid.NewGuid(),
      UserId = paymentDto.UserId,
      LoanId = paymentDto.LoanId,
      Value = paymentDto.Value,
      PaymentStatusTypeId = statusType.Id,
      CreatedAt = DateTime.UtcNow
    };

    _context.Payments.Add(payment);

    var loan = await _context.Loans.FirstOrDefaultAsync(l => l.Id == payment.LoanId);
    if (loan == null)
      throw new Exception("Loan not found.");

    loan.CompletedValue += paymentDto.Value;
    loan.LeftValue -= paymentDto.Value;
    if (loan.LeftValue <= 000001m)
    {
      loan.LoanStatusTypeId = loanStatusType.Id;
      loan.LeftValue = 0;
    }
    else
    if (paymentDto.Value >= loan.ValueToPayOnCurrentMonth)
    {

      loan.NextPaymentDate = loan.NextPaymentDate.AddMonths(1);
      if (loan.LeftValue / (int)loan.Term > loan.LeftValue)
      {
        loan.ValueToPayOnCurrentMonth = loan.LeftValue;
      }
      else
      {
        loan.ValueToPayOnCurrentMonth = loan.ValueToPay * (1 + loan.Percent / 100) / (int)loan.Term;
      }
    }
    else
    {
      loan.ValueToPayOnCurrentMonth -= paymentDto.Value;
    }

    await _context.SaveChangesAsync();

    return new PaymentDTO
    {
      Id = payment.Id,
      UserId = payment.UserId,
      LoanId = payment.LoanId,
      Value = payment.Value,
      Status = statusType.Name,
      CreatedAt = payment.CreatedAt
    };
  }



  public async Task<PaymentDTO> UpdatePaymentAsync(Guid id, PaymentDTO dto)
  {
    var payment = await _context.Payments
      .FirstOrDefaultAsync(p => p.Id == id);

    if (payment == null)
      throw new DatabaseOperationException(Operations.UpdatePayment, new Exception("Payment not found"));

    var statusType = await _context.PaymentStatusTypes
      .FirstOrDefaultAsync(s => s.Name == dto.Status)
      ?? throw new DatabaseOperationException(Operations.UpdatePayment, new Exception("Payment status not found"));

    payment.Value = dto.Value;
    payment.PaymentStatusTypeId = statusType.Id;

    await _context.SaveChangesAsync();

    return new PaymentDTO
    {
      Id = payment.Id,
      UserId = payment.UserId,
      LoanId = payment.LoanId,
      Value = payment.Value,
      Status = statusType.Name,
      CreatedAt = payment.CreatedAt
    };
  }

  public async Task DeletePaymentAsync(Guid id)
  {
    var payment = await _context.Payments.FindAsync(id);
    if (payment == null)
      throw new DatabaseOperationException(Operations.DeletePayment, new Exception("Payment not found"));

    _context.Payments.Remove(payment);
    await _context.SaveChangesAsync();
  }
}
