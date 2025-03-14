using backend.Core.DTOs;
using backend.Core.Models;

public interface ILoanApplicationRepository
    {
        Task<LoanApplicationDTO> GetLoanApplicationAsync(Guid id);
        Task<PaginatedResult<LoanApplicationDTO>> GetLoanApplicationsAsync(PaginationParameters parameters);
        Task<LoanApplicationDTO> CreateLoanApplicationAsync(LoanApplicationDTO loanApplicationDto);
        Task<LoanApplicationDTO> UpdateLoanApplicationAsync(Guid id, LoanApplicationDTO loanApplicationDto);
        Task DeleteLoanApplicationAsync(Guid id);
    }