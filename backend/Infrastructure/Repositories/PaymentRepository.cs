using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Database;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories
{
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
          .FirstOrDefaultAsync(p => p.Id == id);

      if (payment == null)
        throw new DatabaseOperationException(Operations.GetPayment, new Exception("Payment not found"));

      return new PaymentDTO
      {
        Id = payment.Id,
        UserId = payment.UserId,
        Value = payment.Value,
        Status = payment.Status,
        CreatedAt = payment.CreatedAt,
        LoanId = payment.LoanId,
      };
    }

    public async Task<PaginatedResult<PaymentDTO>> GetPaymentsAsync(PaginationParameters parameters)
    {
      var query = _context.Payments.OrderByDescending(p => p.CreatedAt);

      var totalRecords = await query.CountAsync();

      var payments = await query
          .Skip(parameters.Offset * parameters.Size)
          .Take(parameters.Size)
          .Select(payment => new PaymentDTO
          {
            Id = payment.Id,
            UserId = payment.UserId,
            Value = payment.Value,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt,
            LoanId = payment.LoanId,
          })
          .ToListAsync();

      return new PaginatedResult<PaymentDTO>
      {
        Data = payments,
        Total = totalRecords,
      };
    }

    public async Task<PaginatedResult<PaymentDTO>> GetPaymentsByUserIdAsync(Guid userid, PaginationParameters parameters)
    {
      var query = _context.Payments.Where(p => p.UserId == userid).OrderByDescending(p => p.CreatedAt);

      var totalRecords = await query.CountAsync();

      var payments = await query
          .Skip(parameters.Offset * parameters.Size)
          .Take(parameters.Size)
          .Select(payment => new PaymentDTO
          {
            Id = payment.Id,
            UserId = payment.UserId,
            Value = payment.Value,
            Status = payment.Status,
            CreatedAt = payment.CreatedAt,
            LoanId = payment.LoanId,
          })
          .ToListAsync();

      return new PaginatedResult<PaymentDTO>
      {
        Data = payments,
        Total = totalRecords,
      };
    }

    public async Task<PaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto)
    {
      var payment = new Payment
      {
        Id = Guid.NewGuid(),
        UserId = paymentDto.UserId,
        LoanId = paymentDto.LoanId,
        Value = paymentDto.Value,
        Status = PaymentStatus.Completed,
        CreatedAt = DateTime.UtcNow
      };

      _context.Payments.Add(payment);

      var loan = await _context.Loans.FirstOrDefaultAsync(l => l.Id == payment.LoanId);
      if (loan == null)
        throw new Exception("Loan not found.");

      loan.CompletedValue += paymentDto.Value;
      loan.LeftValue -= paymentDto.Value;

      if (paymentDto.Value >= loan.ValueToPayOnCurrentMonth)
      {
        var surplus = paymentDto.Value - loan.ValueToPayOnCurrentMonth;

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
        Status = payment.Status,
        CreatedAt = payment.CreatedAt
      };
    }


    public async Task<PaymentDTO> UpdatePaymentAsync(Guid id, PaymentDTO paymentDto)
    {
      var payment = await _context.Payments
          .FirstOrDefaultAsync(p => p.Id == id);

      if (payment == null)
        throw new DatabaseOperationException(Operations.UpdatePayment, new Exception("Payment not found"));

      payment.Value = paymentDto.Value;
      payment.Status = paymentDto.Status;

      await _context.SaveChangesAsync();

      return new PaymentDTO
      {
        Id = payment.Id,
        UserId = payment.UserId,
        Value = payment.Value,
        LoanId = payment.LoanId,
        Status = payment.Status,
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
}
