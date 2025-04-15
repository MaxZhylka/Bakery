using backend.Core.DTOs;
using backend.Core.Models;

public interface ILoanRepository
    {
        Task<LoanDTO> GetLoanAsync(Guid id);
        Task<PaginatedResult<LoanDTO>> GetLoansAsync(PaginationParameters paginationParameters);
        Task<PaginatedResult<LoanDTO>> GetLoansByUserIdAsync(Guid userId, PaginationParameters paginationParameters);
        Task<LoanDTO> CreateLoanAsync(LoanDTO loan);
        Task<LoanDTO> UpdateLoanAsync(Guid id, LoanDTO loan);
        Task<LoanDTO> GetMoneyByLoanIdAsync(Guid id);
        Task DeleteLoanAsync(Guid id);
    }