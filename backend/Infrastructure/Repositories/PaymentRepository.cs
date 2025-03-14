using backend.Core.DTOs;
using backend.Core.Entities;
using backend.Core.Enums;
using backend.Core.Models;
using backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Core.Exceptions;

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
                CreatedAt = payment.CreatedAt
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
                    CreatedAt = payment.CreatedAt
                })
                .ToListAsync();

            return new PaginatedResult<PaymentDTO>
            {
                Data = payments,
                Total = totalRecords,
            };
        }

        public async Task<PaymentDTO> CreatePaymentAsync(PaymentDTO paymentDto)
        {
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                UserId = paymentDto.UserId,
                Value = paymentDto.Value,
                Status = paymentDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return new PaymentDTO
            {
                Id = payment.Id,
                UserId = payment.UserId,
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
