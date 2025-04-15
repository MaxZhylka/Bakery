using backend.Core.DTOs;
using backend.Core.Models;

public interface ILoanApplicationRepository
    {
        Task<LoanApplicationDTO> GetLoanApplicationAsync(Guid id);
        Task<PaginatedResult<LoanApplicationDTO>> GetLoanApplicationsAsync(PaginationParameters parameters);
        Task<LoanApplicationDTO> CreateLoanApplicationAsync(CreateLoanApplicationDTO loanApplicationDto);
        Task<LoanApplicationDTO> UpdateLoanApplicationAsync(Guid id, LoanApplicationDTO loanApplicationDto);
        Task<PaginatedResult<LoanApplicationDTO>> GetLoanApplicationsByUserIdAsync(Guid userId, PaginationParameters parameters);
        Task ApproveLoanApplicationAsync(Guid id);
        Task RejectLoanApplicationAsync(Guid id, string reason);
        Task DeleteLoanApplicationAsync(Guid id);
    }