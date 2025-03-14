using backend.Core.DTOs;
using backend.Core.Models;

namespace backend.Infrastructure.Repositories
{
    public interface IPaymentRepository
    {
        Task<PaymentDTO> GetPaymentAsync(Guid id);
        Task<PaginatedResult<PaymentDTO>> GetPaymentsAsync(PaginationParameters parameters);
        Task<PaymentDTO> CreatePaymentAsync(PaymentDTO paymentDto);
        Task<PaymentDTO> UpdatePaymentAsync(Guid id, PaymentDTO paymentDto);
        Task DeletePaymentAsync(Guid id);
    }
}
