using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Infrastructure.Repositories
{
    public interface IPaymentRepository
    {
        Task<PaymentDTO> GetPaymentAsync(Guid id);
        Task<PaginatedResult<PaymentDTO>> GetPaymentsAsync(PaginationParameters parameters);
        Task<PaginatedResult<PaymentDTO>> GetPaymentsByUserIdAsync(Guid userId, PaginationParameters parameters);
        Task<PaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto);
        Task<PaymentDTO> UpdatePaymentAsync(Guid id, PaymentDTO paymentDto);
        Task DeletePaymentAsync(Guid id);
    }
}
